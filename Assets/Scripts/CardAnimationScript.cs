using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimationScript : MonoBehaviour
{
    public CardBack cardBack;

    // Start is called before the first frame update
    void Start()
    {
        cardBack = GetComponent<CardBack>();
    }

    public void CardFlip()
    {
        if (cardBack.GetCardBackActive() == true)
        {
            cardBack.UpdateCard(false);
        }
        else
        {
            cardBack.UpdateCard(true);
        }
    }
}
