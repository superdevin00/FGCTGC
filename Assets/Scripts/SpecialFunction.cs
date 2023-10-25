using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFunction : MonoBehaviour
{
    GameController gameController;
    private void Start()
    {
        gameController = GetComponent<GameController>();
    }

    public void checkSpecialFunctionStep(Card card, int frame)
    {
        if (card != null)
        {
            switch (card.cardName)
            {
                case "Solar Plexus Strike": SolarPlexusStrike(frame); break;
                default: break;
            }

        }
    }

    public void checkSpecialFunctionHit(Card card, bool cardHit, string target, bool cardBlocked)
    {
        if (card != null && !cardBlocked)
        {
            switch (card.cardName)
            {
                case "Grab": Grab(cardHit, target); break;
                case "Palm Strike": PalmStrike(cardHit); break;
                case "Leg Sweep": LegSweep(cardHit, target); break;
                case "Flicker Jabs": FlickerJabs(cardHit,target); break;
                default: break;
            }
        }
    }


    public void Grab(bool cardHit, string target)
    {
        if (cardHit)
        {
            gameController.Knockdown(target);
        }
    }
    public void LegSweep(bool cardHit, string target)
    { 
        if (cardHit)
        {
            gameController.Knockdown(target);
        }
    }

    public void PalmStrike(bool cardHit)
    {
        if (cardHit)
        {
            gameController.AddToRange(1);
        }
    }

    public void SolarPlexusStrike(int frame)
    {
        if (frame == 5)
        {
            gameController.AddToRange(-1);
        }
    }

    public void FlickerJabs(bool cardHit, string target)
    {
        if(cardHit)
        {
            //Player attacks
            if (target == "Opponent")
            {
                while(Random.Range(0,2) != 0 && gameController.playerBonusDamage < 100)
                {
                    gameController.playerBonusDamage += gameController.playerCard.damage;
                    gameController.playerBonusAdvantage += gameController.playerCard.hitAdv;
                }
            }
            //Opponent attacks
            /*else if (target == "Player")
            {
                while (Random.Range(0, 2) != 0 && gameController.opponentBonusDamage < 100)
                {
                    gameController.opponentBonusDamage += gameController.opponentCard.damage;
                    gameController.opponentBonusAdvantage += gameController.opponentCard.hitAdv;
                }
            }*/
        }
    }
}
