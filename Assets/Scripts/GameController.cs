using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Card playerCard;
    public Card opponentCard;
    public int currentFrame;
    public int lastFrame;
    public bool playerHit;
    public bool opponentHit;
    private string interactionWinner;

    public int currentRange;

    public TMP_Text winnerText;
    public TMP_Text rangeText;

    SpecialFunction specialFunction;

    // Start is called before the first frame update
    void Start()
    {
        specialFunction = GetComponent<SpecialFunction>();
        currentRange = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rangeText.text = currentRange.ToString();
    }

    public void CheckForWinner()
    {
        playerCard = GameObject.Find("Play Panel").GetComponentInChildren<ThisCard>().thisCard[0];
        opponentCard = GameObject.Find("Opponent Card Panel").GetComponentInChildren<ThatCard>().thatCard[0];
        playerHit = false;
        opponentHit = false;


        //Set the last frame we check to the card that has the longest recovery
        if (playerCard.recovery >= opponentCard.recovery)
        {
            lastFrame = playerCard.recovery;
        }
        else
        {
            lastFrame = opponentCard.recovery;
        }

        for (int frame = 0; frame < lastFrame; frame++) //Step thru frames to determine winner
        {
            if (playerHit || opponentHit) //Exit loop if hit already landed
            {
                break;
            }
            specialFunction.checkSpecialFunctionStep(playerCard, frame);
            specialFunction.checkSpecialFunctionStep(opponentCard, frame);

            if(playerCard.startUp == frame && playerCard.range >= currentRange)
            {
                playerHit = true;
            }
            if(opponentCard.startUp == frame && opponentCard.range >= currentRange)
            {
                opponentHit = true;
            }
        }

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

        /*if (playerCard.startUp < opponentCard.startUp)
        {
            interactionWinner = "Player";
            playerHit = true;
            opponentHit = false;
        }
        else if (playerCard.startUp > opponentCard.startUp)
        {
            interactionWinner = "Opponent";
            playerHit = false;
            opponentHit = true;
        }
        else
        {
            interactionWinner = "Draw";
            playerHit = true;
            opponentHit = true;
        }*/

        //Check for any special functions that trigger on hit/whiff
        specialFunction.checkSpecialFunctionHit(playerCard, playerHit);
        specialFunction.checkSpecialFunctionHit(opponentCard, opponentHit);

        winnerText.text = interactionWinner;
    }

    public void AddToRange (int x) //Alter the current range by X
    {
        currentRange = Mathf.Clamp(currentRange + x, 0, 6); //Ensure CurrentRange is within min and max values 
    }
}
