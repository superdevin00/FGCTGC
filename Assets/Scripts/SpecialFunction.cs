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
        switch (card.cardName)
        {
            case "Solar Plexus Strike": SolarPlexusStrike(frame); break;
            default: break;
        }
    }

    public void checkSpecialFunctionHit(Card card, bool cardHit)
    {
        switch (card.cardName)
        {
            case "Palm Strike": PalmStrike(cardHit); break;
            default: break;
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
}
