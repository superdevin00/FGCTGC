using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class GameController : MonoBehaviour
{
    public Card playerCard;
    public Card opponentCard;
    public GameObject playerCardObject;
    public GameObject opponentCardObject;
    public PlayerDeck playerDeck;
    public OpponentDeck oppDeck;
    public int playerAdv;

    public int playerTurnSync;
    public int opponentTurnSync;
    public int playerNeutralSync;
    public int opponentNeutralSync;

    public int playerHealth;
    public int opponentHealth;

    public int currentFrame;
    public int lastFrame;
    public int playerFrameToContinue;
    public int opponentFrameToContinue;

    public bool playerHit;
    public bool opponentHit;
    public bool playerBlock;
    public bool opponentBlock;

    private string interactionWinner;
    public string gamePhase;
    public bool isKnockdown;
    public bool canPlayerPlayCard;
    public bool canOpponentPlayCard;

    public int currentRange;

    public TMP_Text winnerText;
    public TMP_Text rangeText;
    public TMP_Text advantageText;

    public GameObject checkButton;
    public TMP_Text checkButtonText;
    public GameObject returnToNeutralButton;

    public GameObject waitingForOpponentText;
    public GameObject playerPlayPanel;
    public GameObject opponentPlayPanel;

    public bool isCheckingWinner;
    public bool isSameWinCheck;
    public int checkWinnerStep;
    public int opponentCheckWinnerStep;
    public bool isOpponentCheckingWinner;
    public bool isWaitingForOpponent;
    public bool playerReady;
    public bool opponentReady;
    public bool playerStunned;
    public int opponentCardID;
    public TMP_Text readyText;


    SpecialFunction specialFunction;

    // Start is called before the first frame update
    void Start()
    {
        playerStunned = false;
        isCheckingWinner = false;
        checkWinnerStep = 0;
        oppDeck = GameObject.Find("Opponent Deck Panel").GetComponent<OpponentDeck>();
        playerDeck = GameObject.Find("Deck Panel").GetComponent<PlayerDeck>();
        specialFunction = GetComponent<SpecialFunction>();
        gamePhase = "Neutral";
        currentRange = 0;
        playerHealth = 100;
        opponentHealth = 100;
        playerReady = false;
        isWaitingForOpponent = true;
        canPlayerPlayCard = true;
    }

    // Update is called once per frame
    void Update()
    {
        advantageText.text = (playerAdv > 0) ? "+" + playerAdv : playerAdv.ToString();
        rangeText.text = currentRange.ToString();
        UpdateCheckWinnerButton();
        UpdateNeutralButton();

        //Set splash text
        if (playerStunned)
        {
            readyText.text = "Stunned!";
            readyText.gameObject.SetActive(true);
        }
        else if (playerReady)
        {
            readyText.text = "Player Ready";
            readyText.gameObject.SetActive(true);
        }
        else
        {
            readyText.gameObject.SetActive(false);
        }

        //Check if waiting for opponent to join
        if (NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetcodeHandler>().opponent != null)
        {
            isWaitingForOpponent = false;
        }
        waitingForOpponentText.SetActive(isWaitingForOpponent);

        //Place card when opponent plays
        if (opponentReady && canOpponentPlayCard && GameObject.Find("Opponent Card Panel").transform.childCount == 0 && !isCheckingWinner && !isOpponentCheckingWinner && checkWinnerStep == 0)
        {
            Debug.Log("TryPlace");
            oppDeck.playOpponentCard(opponentCardID);
        }

        //set player card stuff
        if (playerPlayPanel.transform.childCount != 0 && playerPlayPanel.GetComponentInChildren<ThisCard>() != null)
        {
            playerCard = GameObject.Find("Play Panel").GetComponentInChildren<ThisCard>().thisCard[0];
            playerCardObject = GameObject.Find("Play Panel").GetComponentInChildren<ThisCard>().gameObject;
        }
        else
        {
            playerCard = null;
            playerCardObject = null;
        }
        if (opponentPlayPanel.transform.childCount !=0 && opponentPlayPanel.GetComponentInChildren<ThatCard>() != null)
        {
            opponentCard = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().thatCard[0];
            opponentCardObject = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().gameObject;
        }
        else
        {
            opponentCard = null;
            opponentCardObject = null;
        }

        //Check if opponent returned to neutral
        if(playerStunned && opponentNeutralSync == playerNeutralSync + 1)
        {
            ManualReturnToNeutral();
        }
        //Check if player is stunned to see if it will be a mixup or combo
        else if (playerStunned && (opponentReady || isOpponentCheckingWinner) && playerTurnSync + 1 == opponentTurnSync)
        {
            Debug.Log("CheckThisMix");
            CheckForMixup();
        }

        //When all players ready, check who won
        if (playerReady && (opponentReady||isOpponentCheckingWinner) && !isCheckingWinner && !isSameWinCheck && playerTurnSync == opponentTurnSync && !playerStunned)
        {
            StartCoroutine(CheckForWinnerRoutine());
        }
    }

    public void UpdateCheckWinnerButton()
    {
        //Update button for neutral phase
        if (isWaitingForOpponent)
        {
            checkButton.SetActive(false);
            //canPlayerPlayCard = false;
        }
        else if (gamePhase == "Neutral" || gamePhase == "Knockdown")
        {
            //Make button visible if both player and opponent have card played
            if (playerPlayPanel.transform.childCount > 0)
            {
                checkButton.SetActive(true);
                checkButtonText.text = "Play";
            }
            else
            {
                checkButton.SetActive(false);
            }
        }
        else if (gamePhase == "Punish")
        {
            if (playerAdv > 0)
            {
                if (playerCard != null)
                {              
                    //Check if it will be a true combo
                    if (playerCard.startUp <= playerAdv && playerCard.cardType != "Block" && playerCard.cardType != "Grab")
                    {
                        checkButton.SetActive(true);
                        checkButtonText.text = "Combo";
                    }    
                    else
                    {
                        checkButton.SetActive(true);
                        checkButtonText.text = "Mixup";
                    }
                }
                else
                {
                    checkButton.SetActive(false);
                }
            }
        }
    }
    public void UpdateNeutralButton()
    {
        //Update button for neutral phase
        if (isWaitingForOpponent || playerReady || playerStunned || gamePhase == "Neutral")
        {
            returnToNeutralButton.SetActive(false);
        }
        else if (gamePhase == "Punish" || gamePhase == "Knockdown")
        {
            returnToNeutralButton.SetActive(true);
        }
    }

    public void PlayerReady()
    {
        playerTurnSync += 1;
        canPlayerPlayCard = false;
        playerReady = true;
    }

    public IEnumerator CheckForWinnerRoutine()
    {

        Debug.Log("CheckWinner");
        isCheckingWinner = true;
        checkWinnerStep = 1;

        while (!isOpponentCheckingWinner && opponentReady && opponentCheckWinnerStep < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        isSameWinCheck = true;
        playerReady = false;



        playerHit = false;
        opponentHit = false;
        playerBlock = false;
        opponentBlock = false;

        if(gamePhase == "Neutral" || gamePhase == "Knockdown" || gamePhase == "Mixup")
        {
            yield return new WaitUntil(NeutralPhase);
            Debug.Log("Moving on");
        }
        else if (gamePhase == "Punish")
        {
            yield return new WaitUntil(PunishPhase);

            //If player is doing mixup
            if(gamePhase == "Mixup")
            {
                yield return new WaitUntil(NeutralPhase);
            }
        }


        //Determine the interaction winner text
        if (gamePhase == "Mixup")
        {
            interactionWinner = "Mixup";
        }
        else if (playerHit && opponentHit)
        {
            interactionWinner = "Trade";
        }
        else if (playerBlock || opponentBlock)
        {
            interactionWinner = "Attack Blocked";
        }
        else if (playerHit)
        {
            interactionWinner = "Player";
        }
        else if (opponentHit)
        {
            interactionWinner = "Opponent";
        }
        else
        {
            interactionWinner = "Miss";
        }

        //Deal Damage on hit
        if(playerHit)
        {
            //Check if it should do regular dmg or block chip
            if (opponentBlock)
            {
                DamageOpponent(Mathf.RoundToInt((playerCard.damage / 2) + 0.5f)); //Deal half dmg rounded up on block
            }
            else
            {
                DamageOpponent(playerCard.damage);
            }
        }
        if(opponentHit)
        {
            //Check if it should do regular dmg or block chip
            if (playerBlock)
            {
                DamagePlayer(Mathf.RoundToInt((opponentCard.damage / 2) + 0.5f)); //Deal half dmg rounded up on block
            }
            else
            {
                DamagePlayer(opponentCard.damage);
            }
        }

        specialFunction.checkSpecialFunctionHit(playerCard, playerHit, "Opponent", opponentBlock);
        specialFunction.checkSpecialFunctionHit(opponentCard, opponentHit, "Player", playerBlock);


        winnerText.text = interactionWinner;
        isCheckingWinner = false;
        checkWinnerStep = 2;

        while (isOpponentCheckingWinner && opponentCheckWinnerStep < 2)
        {
            Debug.Log("opponent still checking");
            yield return new WaitForSeconds(0.1f);
        }
        if(!isOpponentCheckingWinner)
        {
            isSameWinCheck = false;
            Debug.Log("Try Discard");
            /*if (gamePhase != "Mixup")
            {*/
                if (playerCard != null)
                {
                    playerCardObject.GetComponent<ThisCard>().DiscardCard();
                }
                if (opponentCard != null)
                {
                    opponentCardObject.GetComponent<ThatCard>().DiscardCard();
                }
            //}
            DetermineNextGamePhase();
        }

        //Prevent Accidental Opp Card Placement
        checkWinnerStep = 3;
        while(opponentCheckWinnerStep < 3 && isOpponentCheckingWinner)
        {
            yield return new WaitForSeconds(0.1f);
        }
        checkWinnerStep = 0;
    }

    public void DetermineNextGamePhase()
    {
        Debug.Log("DetermineNextGamePhase");

        //playerAdv = (playerHit ? playerCard.hitAdv : 0) - (opponentHit ? opponentCard.hitAdv : 0);

        // SET PLAYER ADV
        //Retain player adv if mixup
        if (gamePhase != "Mixup")
        {
            //If only the player hit
            if (playerHit && !opponentHit)
            {
                //If it was blocked, give block adv
                if (opponentBlock)
                {
                    playerAdv = playerCard.blockAdv;
                }
                //If not, give hit adv
                else
                {
                    playerAdv = playerCard.hitAdv;
                }
            }
            //If only opponent hit
            else if (!playerHit && opponentHit)
            {
                //If it was blocked, give block adv
                if (playerBlock)
                {
                    playerAdv = -opponentCard.blockAdv;
                }
                //If not, give hit adv
                else
                {
                    playerAdv = -opponentCard.hitAdv;
                }
            }
            else if (playerHit && opponentHit && !(playerBlock || opponentBlock))
            {
                playerAdv = playerCard.hitAdv - opponentCard.hitAdv;
            }
        }

        //Determine next phase
        if (isKnockdown)
        {
            gamePhase = "Knockdown";
            isKnockdown = false;
            ReturnToNeutral();
        }
        else if (playerHit || opponentHit)
        {
            if (playerAdv != 0)
            {
                gamePhase = "Punish";
            }
            else
            {
                gamePhase = "Neutral";
                ReturnToNeutral();
            }
        }
        else
        {
            //Default case
            gamePhase = "Neutral";
            playerAdv = 0;
            ReturnToNeutral();
        }

        if (playerAdv >= 0)
        {
            canPlayerPlayCard = true;
        }
        else if (gamePhase != "Knockdown")
        {
            //force player stun if they are negative and not knockdown
            playerStunned = true;
            canPlayerPlayCard = false;
        }
    }

    public void ReturnToNeutral()
    {
        playerDeck.DrawTo(7);
    }

    public void ManualReturnToNeutral()
    {
        playerNeutralSync++;
        playerAdv = 0;
        gamePhase = "Neutral";
        playerStunned = false;
        playerDeck.DrawTo(7);
        canPlayerPlayCard = true;
    }

    public void Knockdown(string target)
    {
        isKnockdown = true;
        if (target == "Player")
        {
            playerAdv -= opponentCard.hitAdv;
            //opponentAdv += opponentCard.hitAdv;
        }
        else if (target == "Opponent")
        {
            playerAdv += playerCard.hitAdv;
            //opponentAdv -= playerCard.hitAdv;
        }
    }

    public void DamagePlayer(int dmg)
    {
        playerHealth -= dmg;
    }
    public void DamageOpponent(int dmg)
    {
        opponentHealth -= dmg;
    }
    public void SetPlayerHealth(int hp)
    {
        playerHealth = hp;
    }
    public void SetOpponentHealth(int hp)
    {
        opponentHealth = hp;
    }

    public void AddToRange (int x) //Alter the current range by X
    {
        currentRange = Mathf.Clamp(currentRange + x, 0, 6); //Ensure CurrentRange is within min and max values 
    }

    public bool NeutralPhase()
    {
        Debug.Log("NeutralPhase");
        //Get Cards
        playerCard = GameObject.Find("Play Panel").GetComponentInChildren<ThisCard>().thisCard[0];
        playerCardObject = GameObject.Find("Play Panel").GetComponentInChildren<ThisCard>().gameObject;

        if (GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>() != null)
        {
            opponentCard = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().thatCard[0];
            opponentCardObject = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().gameObject;
        }
        else
        {
            if (canOpponentPlayCard && GameObject.Find("Opponent Card Panel").transform.childCount == 0 && opponentCardID != -1)
            {
                oppDeck.playOpponentCard(opponentCardID);
            }
            return false;
        }

        //Set Advantage to frame before startup if startup is faster than advantage for whatever reason
        if (playerAdv > 0 && playerCard.startUp <= playerAdv)
        {
            playerAdv = playerCard.startUp - 1;
        }
        else if(playerAdv < 0 && opponentCard.startUp <= -playerAdv)
        {
            playerAdv = -(opponentCard.startUp - 1);
        }

        //Set the last frame we check to the card that has the longest recovery
        if (playerCard.recovery + (playerAdv < 0? -playerAdv : 0) >= opponentCard.recovery + (playerAdv > 0? playerAdv:0))
        {
            lastFrame = playerCard.recovery + (playerAdv < 0 ? -playerAdv : 0);
        }
        else
        {
            lastFrame = opponentCard.recovery + (playerAdv > 0 ? playerAdv : 0);
        }

        //Step thru frames to determine winner
        for (int frame = 0; frame < lastFrame; frame++)
        {
            //Exit loop if hit already landed
            if (playerHit || opponentHit)
            {
                break;
            }

            //Check for special functions that occur per frame
            specialFunction.checkSpecialFunctionStep(playerCard, frame);
            specialFunction.checkSpecialFunctionStep(opponentCard, frame);

            //Check if within range during startup frame
            if (playerCard.startUp + (playerAdv < 0 ? -playerAdv : 0) == frame && playerCard.range >= currentRange && playerCard.cardName != "Block" && playerCard.damage > 0)
            {
                playerHit = true;
            }
            if (opponentCard.startUp + (playerAdv > 0 ? playerAdv : 0) == frame && opponentCard.range >= currentRange && opponentCard.cardName != "Block" && opponentCard.damage > 0)
            {
                opponentHit = true;
            }

            //Check if attack was blocked
            if (playerHit && opponentCard.cardName == "Block")
            {
                opponentBlock = true;
            }
            if (opponentHit && playerCard.cardName == "Block")
            {
                playerBlock = true;
            }

            //Check grab logic
            if(playerCard.cardName == "Grab" && opponentCard.cardName == "Grab")
            {
                playerHit = false;
                opponentHit = false;
                break;
            }
            else if (playerCard.cardName == "Grab")
            {
                if (opponentBlock)
                {
                    opponentBlock = false;
                }

                //Grab loses if atack lands on same frame
                if (opponentHit)
                {
                    playerHit = false;
                }
            }
            else if (opponentCard.cardName == "Grab")
            {
                if (playerBlock)
                {
                    playerBlock = false;
                }

                //Grab loses if atack lands on same frame
                if (playerHit)
                {
                    opponentHit = false;
                }
            }
        }
        return true;
    }


    public bool PunishPhase()
    {
        Debug.Log("PunishPhase");
        if (playerAdv > 0)
        {
            playerCard = GameObject.Find("Play Panel").GetComponentInChildren<ThisCard>().thisCard[0];
            playerCardObject = GameObject.Find("Play Panel").GetComponentInChildren<ThisCard>().gameObject;

            if (playerCard.startUp <= playerAdv && playerCard.cardType != "Block" && playerCard.cardType != "Grab")
            {
                Debug.Log("Combo");
                lastFrame = playerCard.recovery + (playerAdv < 0 ? -playerAdv : 0);

                //Step thru frames to determine winner
                for (int frame = 0; frame < lastFrame; frame++)
                {
                    //Exit loop if hit already landed
                    if (playerHit)
                    {
                        break;
                    }

                    //Check for special functions that occur per frame
                    specialFunction.checkSpecialFunctionStep(playerCard, frame);

                    //Check if within range during startup frame
                    if (playerCard.startUp == frame && playerCard.range >= currentRange && playerCard.cardName != "Block" && playerCard.damage > 0)
                    {
                        playerHit = true;
                    }
                }        
            }
            else
            {
                Debug.Log("Mixup");
                gamePhase = "Mixup";

            }
        }
        else if (playerAdv < 0)
        {
            if (GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>() != null)
            {
                opponentCard = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().thatCard[0];
                opponentCardObject = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().gameObject;
            }
            else
            {
                if (canOpponentPlayCard && GameObject.Find("Opponent Card Panel").transform.childCount == 0 && opponentCardID != -1)
                {
                    oppDeck.playOpponentCard(opponentCardID);
                }
                return false;
            }
            //oppDeck.Draw(1);
            opponentCard = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().thatCard[0];
            opponentCardObject = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().gameObject;

            if (opponentCard.startUp <= -playerAdv)
            {


                //Set the last frame we check to the card that has the longest recovery
                lastFrame = opponentCard.recovery + (playerAdv > 0 ? playerAdv : 0);

                //Step thru frames to determine winner
                for (int frame = 0; frame < lastFrame; frame++)
                {
                    //Exit loop if hit already landed
                    if (opponentHit)
                    {
                        break;
                    }

                    //Check for special functions that occur per frame
                    specialFunction.checkSpecialFunctionStep(opponentCard, frame);

                    //Check if within range during startup frame
                    if (opponentCard.startUp + (playerAdv > 0 ? playerAdv : 0) == frame && opponentCard.range >= currentRange && opponentCard.cardName != "Block" && opponentCard.damage > 0)
                    {
                        opponentHit = true;
                    }
                }
            }
            else
            {
                Debug.Log("Mixup");
                canPlayerPlayCard = true;
                gamePhase = "Mixup";
            }
        }
        return true;
    }

    public void CheckForMixup()
    {
        if (GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>() != null)
        {
            opponentCard = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().thatCard[0];
            opponentCardObject = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().gameObject;
        }
        if (opponentCard != null)
        {
            if (Mathf.Abs(playerAdv) >= opponentCard.startUp)
            {
                //Combo
                PlayerReady();
                playerStunned = false;
                gamePhase = "Punish";
            }
            else
            {
                //Mixup
                playerStunned = false;
                canPlayerPlayCard = true;
                gamePhase = "Mixup";
            }
        }
    }
}
