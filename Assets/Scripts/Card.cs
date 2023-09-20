using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Card
{
    public int id; [Tooltip("Card ID")]
    public string cardName;
    public int[] cardStyles;
    public int startUp;
    public int recovery;
    public int damage;
    public int range;
    public string description;

    public Sprite cardImage;

    public string cardType;
    

    public Card()
    {

    }

    public Card(int Id, string CardName, int[] CardStyles, int StartUp, int Recovery, int Damage, int Range, string Description, Sprite CardImage, string CardType)
    {
        id = Id;
        cardName = CardName;
        cardStyles = CardStyles;
        startUp = StartUp;
        recovery = Recovery;
        range = Range;
        description = Description;
        cardImage = CardImage;
        cardType = CardType;
    }
}
