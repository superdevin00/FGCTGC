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
    public int hitAdv;
    public int blockAdv;
    public string description;

    public Sprite cardImage;

    public string cardType;
    

    public Card()
    {

    }

    public Card(int Id, string CardName, int[] CardStyles, int StartUp, int Recovery, int Damage, int Range,int HitAdv, int BlockAdv, string Description, Sprite CardImage, string CardType)
    {
        id = Id;
        cardName = CardName;
        cardStyles = CardStyles;
        startUp = StartUp;
        recovery = Recovery;
        damage = Damage;
        range = Range;
        hitAdv = HitAdv;
        blockAdv = BlockAdv;
        description = Description;
        cardImage = CardImage;
        cardType = CardType;
    }
}
