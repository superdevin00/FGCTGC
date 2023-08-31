using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ThisCard : MonoBehaviour
{
    public List<Card> thisCard = new List<Card>();
    public int thisId;

    public int id;
    public string cardName;
    public int[] cardStyles;
    public int startUp;
    public int recovery;
    public int damage;
    public int range;
    public string description;

    public TMP_Text nameText;
    public Image style1Image;
    public Image style2Image;
    public TMP_Text startUpText;
    public TMP_Text recoveryText;
    public TMP_Text damageText;
    public TMP_Text rangeText;
    public TMP_Text descriptionText;

    public Sprite thisSprite;
    public Image thisImage;
    public Image frame;
    private Color frameColor;

    public bool cardBack;
    public static bool staticCardBack;
    CardBack cardBackScript;

    public GameObject Hand;

    public PlayerDeck playerDeck;
    public int numberOfCardsInDeck;

    // Start is called before the first frame update
    void Start()
    {
        playerDeck = GameObject.Find("Deck Panel").GetComponent<PlayerDeck>();
        thisCard[0] = CardDataBase.cardList[thisId];
        numberOfCardsInDeck = playerDeck.deckSize;
    }

    // Update is called once per frame
    void Update()
    {
        Hand = GameObject.Find("Hand Panel");
        if(this.transform.parent == Hand.transform)
        {
            cardBack = false;

        }

        cardBackScript = GetComponent<CardBack>();
        id = thisCard[0].id;
        cardName = thisCard[0].cardName;
        cardStyles = thisCard[0].cardStyles;
        startUp = thisCard[0].startUp;
        recovery = thisCard[0].recovery;
        damage = thisCard[0].damage;
        range = thisCard[0].range;
        description = thisCard[0].description;

        thisSprite = thisCard[0].cardImage;

        nameText.text = "" + cardName;
        startUpText.text = "" + startUp;
        recoveryText.text = "" + recovery;
        descriptionText.text = description;
        //damageText.text = "" + damage;
        //rangeText.text = "" + range;

        thisImage.sprite = thisSprite;

        switch (thisCard[0].cardType)
        {
            case "None": frameColor = Color.gray; break;
            case "Character": frameColor = Color.black; break;
            case "Attack": frameColor = Color.red; break;
            case "Block": frameColor = Color.blue; break;
            case "Grab": frameColor = Color.green; break;

            default: frameColor = Color.white; break;
        }

        frame.GetComponent<Image>().color = frameColor;

        cardBackScript.UpdateCard(cardBack);
        staticCardBack = cardBack;

        if(this.tag == "Clone")
        {
            thisCard[0] = PlayerDeck.staticDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
            playerDeck.deckSize -= 1;
            this.tag = "Untagged";
        }
    }
}
