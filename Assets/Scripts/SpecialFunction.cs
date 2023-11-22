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
                case "Quick Step": QuickStep(frame); break;
                case "Dash Punch": DashPunch(frame); break;
                case "Swaying Strike": SwayingStrike(frame); break;
                case "Flying Dropkick": FlyingDropkickStep(frame); break;
                default: break;
            }

        }
    }

    public void checkSpecialFunctionHit(Card card, bool cardHit, string target, bool cardBlocked)
    {
        if (card != null)
        {
            switch (card.cardName)
            {
                case "Grab": Grab(cardHit, target); break;
                case "Palm Strike": PalmStrike(cardHit); break;
                case "Leg Sweep": LegSweep(cardHit, target, cardBlocked); break;
                case "Flicker Jabs": FlickerJabs(cardHit,target); break;
                case "Flying Dropkick": FlyingDropkickHit(cardHit, target,cardBlocked); break;
                case "Grapple Hook": GrappleHook(cardHit, cardBlocked); break;
                case "Kunai & Chain": KunaiAndChain(cardHit, cardBlocked); break;
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
    public void LegSweep(bool cardHit, string target, bool cardBlocked)
    { 
        if (cardHit && !cardBlocked)
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

    public void QuickStep(int frame)
    {
        if (frame == 1 || frame == 5)
        {
            gameController.AddToRange(-1);
        }
    }

    public void DashPunch(int frame)
    {
        if (frame == 5)
        {
            gameController.AddToRange(-2);
        }
    }

    public void SwayingStrike (int frame)
    {
        if (frame == 3)
        {
            gameController.AddToRange(1);
        }
        if (frame == 7)
        {
            gameController.AddToRange(-1);
        }
    }

    public void FlyingDropkickStep (int frame)
    {
        if (frame == 1 || frame == 7)
        {
            gameController.AddToRange(-1);
        }
    }

    public void FlyingDropkickHit (bool cardHit, string target, bool cardBlocked)
    {
        if (cardHit && !cardBlocked)
        {
            gameController.Knockdown(target);
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

    public void GrappleHook(bool cardHit, bool cardBlocked)
    {
        if (cardHit && !cardBlocked)
        {
            gameController.AddToRange(-2);
        }
        else if (cardHit && cardBlocked)
        {
            gameController.AddToRange(-1);
        }
    }
    public void KunaiAndChain(bool cardHit, bool cardBlocked)
    {
        if (cardHit && !cardBlocked)
        {
            gameController.AddToRange(2);
        }
        else if (cardHit && cardBlocked)
        {
            gameController.AddToRange(1);
        }
    }
}
