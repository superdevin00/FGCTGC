using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public int currentRange;

    public TMP_Text winnerText;
    public TMP_Text rangeText;
    public TMP_Text advantageText;

    public GameObject checkButton;
    public TMP_Text checkButtonText;
    public GameObject playerPlayPanel;
    public GameObject opponentPlayPanel;


    SpecialFunction specialFunction;

    // Start is called before the first frame update
    void Start()
    {
        oppDeck = GameObject.Find("Opponent Deck Panel").GetComponent<OpponentDeck>();
        playerDeck = GameObject.Find("Deck Panel").GetComponent<PlayerDeck>();
        specialFunction = GetComponent<SpecialFunction>();
        gamePhase = "Neutral";
        currentRange = 0;
        playerHealth = 100;
        opponentHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
       advantageText.text = (playerAdv > 0) ? "+" + playerAdv : playerAdv.ToString();
       rangeText.text = currentRange.ToString();
       UpdateCheckWinnerButton();
    }

    public void UpdateCheckWinnerButton()
    {
        //Update button for neutral phase
        if (gamePhase == "Neutral" || gamePhase == "Knockdown")
        {
            //Make button visible if both player and opponent have card played
            if (playerPlayPanel.transform.childCount > 0 && opponentPlayPanel.transform.childCount > 0)
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
                if (playerPlayPanel.transform.childCount > 0)
                {
                    playerCard = GameObject.Find("Play Panel").GetComponentInChildren<ThisCard>().thisCard[0];
                    playerCardObject = GameObject.Find("Play Panel").GetComponentInChildren<ThisCard>().gameObject;
                    
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

    public void CheckForWinner()
    {

        playerHit = false;
        opponentHit = false;
        playerBlock = false;
        opponentBlock = false;

        if(gamePhase == "Neutral" || gamePhase == "Knockdown")
        {
            NeutralPhase();
        }
        else if (gamePhase == "Punish")
        {
            PunishPhase();
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

        

        DetermineNextGamePhase();
    }

    public void DetermineNextGamePhase()
    {
        playerAdv = (playerHit ? playerCard.hitAdv : 0) - (opponentHit ? opponentCard.hitAdv : 0);

        if (isKnockdown)
        {
            gamePhase = "Knockdown";
            isKnockdown = false;

            //Discard Cards
            playerCardObject.GetComponent<ThisCard>().DiscardCard();
            Destroy(opponentCardObject);

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

            playerCardObject.GetComponent<ThisCard>().DiscardCard();
            Destroy(opponentCardObject);


        }
        else
        {
            gamePhase = "Neutral";
            playerAdv = 0;
            //opponentAdv = 0;

            playerCardObject.GetComponent<ThisCard>().DiscardCard();
            Destroy(opponentCardObject);

            ReturnToNeutral();
        }
    }

    public void ReturnToNeutral()
    {
        oppDeck.Draw(1);
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

    public void NeutralPhase()
    {
        //Get Cards
        playerCard = GameObject.Find("Play Panel").GetComponentInChildren<ThisCard>().thisCard[0];
        opponentCard = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().thatCard[0];
        playerCardObject = GameObject.Find("Play Panel").GetComponentInChildren<ThisCard>().gameObject;
        opponentCardObject = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().gameObject;

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
    }

    public void PunishPhase()
    {
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
                oppDeck.Draw(1);
                NeutralPhase();
            }
        }
        else if (playerAdv < 0)
        {
            oppDeck.Draw(1);
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
                playerDeck.DrawTo(7);
                NeutralPhase();
            }
        }

    }

    public void MixupPhase()
    {

    }
}
