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
    public int opponentCardID;
    public TMP_Text readyText;


    SpecialFunction specialFunction;

    // Start is called before the first frame update
    void Start()
    {
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
        readyText.gameObject.SetActive(playerReady);
        
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

        //Get rid of a card that isnt supposed to be there
        /*if (canOpponentPlayCard == false && GameObject.Find("Opponent Card Panel").transform.childCount > 0 && !isCheckingWinner && !isOpponentCheckingWinner)
        {
            Debug.Log("Cleanup");
            GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().DiscardCard();
        }*/

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

        //When all players ready, check who won
        if (playerReady && (opponentReady||isOpponentCheckingWinner) && !isCheckingWinner && !isSameWinCheck)
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
            if (playerPlayPanel.transform.childCount > 0 /*&& opponentPlayPanel.transform.childCount > 0*/)
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
                    if (playerCard.startUp <= playerAdv)
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

    public void PlayerReady()
    {
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

        if(gamePhase == "Neutral" || gamePhase == "Knockdown")
        {
            yield return new WaitUntil(NeutralPhase);
            Debug.Log("Moving on");
        }
        else if (gamePhase == "Punish")
        {
            yield return new WaitUntil(PunishPhase);
        }


        //Determine the interaction winner text
        if (playerHit && opponentHit)
        {
            interactionWinner = "Trade";
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

        //Check for any special functions that trigger on hit/whiff
        specialFunction.checkSpecialFunctionHit(playerCard, playerHit, "Opponent");
        specialFunction.checkSpecialFunctionHit(opponentCard, opponentHit, "Player");

        winnerText.text = interactionWinner;
/*        
        while (isOpponentCheckingWinner) 
        {
            yield return new WaitForSeconds(0.1f);
        }*/

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
            if (playerCard != null)
            {
                playerCardObject.GetComponent<ThisCard>().DiscardCard();
            }
            if (opponentCard != null)
            {
                opponentCardObject.GetComponent<ThatCard>().DiscardCard();
            }
            DetermineNextGamePhase();
        }

        //Prevent Accidental Opp Card Placement
        checkWinnerStep = 3;
        while(opponentCheckWinnerStep < 3)
        {
            yield return new WaitForSeconds(0.1f);
        }
        checkWinnerStep = 0;
    }

    public void DetermineNextGamePhase()
    {
        Debug.Log("DetermineNextGamePhase");
        playerAdv = (playerHit ? playerCard.hitAdv : 0) - (opponentHit ? opponentCard.hitAdv : 0);

        if (isKnockdown)
        {
            gamePhase = "Knockdown";
            isKnockdown = false;
            ReturnToNeutral();
        }
        else if (playerHit || opponentHit && !(playerBlock || opponentBlock))
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
            //force player ready if they are negative and not knockdown
            PlayerReady();
        }



    }

    public void ReturnToNeutral()
    {
        //oppDeck.Draw(1);
        playerDeck.DrawTo(7);
        
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
            if (playerCard.startUp + (playerAdv < 0 ? -playerAdv : 0) == frame && playerCard.range >= currentRange && playerCard.cardName != "Block")
            {
                playerHit = true;
            }
            if (opponentCard.startUp + (playerAdv > 0 ? playerAdv : 0) == frame && opponentCard.range >= currentRange && opponentCard.cardName != "Block")
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

            if (playerCard.startUp <= playerAdv)
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
                    if (playerCard.startUp == frame && playerCard.range >= currentRange && playerCard.cardName != "Block")
                    {
                        playerHit = true;
                    }
                }        
            }
            else
            {
                Debug.Log("Mixup");
                NeutralPhase();
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
                    if (opponentCard.startUp + (playerAdv > 0 ? playerAdv : 0) == frame && opponentCard.range >= currentRange && opponentCard.cardName != "Block")
                    {
                        opponentHit = true;
                    }
                }
            }
            else
            {
                Debug.Log("Mixup");
                canPlayerPlayCard = true;
                NeutralPhase();
            }
        }
        return true;
    }

    public void MixupPhase()
    {

    }
}
