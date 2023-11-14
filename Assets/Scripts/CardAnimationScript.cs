using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimationScript : MonoBehaviour
{
    public CardBack cardBack;
    public ThisCard thisCard;
    public ThatCard thatCard;

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

    public void CardAttack()
    {
        Debug.Log("CheckCardAttack");
        if (thisCard != null)
        {
            Debug.Log("ThisCard Found");
            //ThisCard thisCard = gameObject.GetComponent<ThisCard>();
            thisCard.cardAlreadyAttacked = true;
        }
        else if (thatCard != null)
        {
            //ThatCard thatCard = gameObject.GetComponent<ThatCard>();
            thatCard.cardAlreadyAttacked = true;
        }
    }
}
