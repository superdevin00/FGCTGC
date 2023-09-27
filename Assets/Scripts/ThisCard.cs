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
    public int hitAdv;
    public int blockAdv;
    public string description;

    public TMP_Text nameText;
    public Image style1Image;
    public Image style2Image;
    public TMP_Text startUpText;
    public TMP_Text recoveryText;
    public TMP_Text damageText;
    public TMP_Text rangeText;
    public TMP_Text hitAdvText;
    public TMP_Text blockAdvText;
    public TMP_Text descriptionText;

    public Sprite thisSprite;
    public Image thisImage;
    public Image frame;
    private Color frameColor;

    public GameObject attackStats;

    public bool cardBack;
    public static bool staticCardBack;
    CardBack cardBackScript;

    public GameObject Hand;

    public PlayerDeck playerDeck;
    public int numberOfCardsInDeck;

    public GameObject playZone;

    public bool canBePlayed;
    public bool playerCanPlay;

    public GameObject Discard;
    public bool isInDiscard;
    public GameController gameController;
    public TurnSystem turnSystem;

    // Start is called before the first frame update
    void Start()
    {
        playerDeck = GameObject.Find("Deck Panel").GetComponent<PlayerDeck>();
        turnSystem = GameObject.Find("TurnSystemController").GetComponent<TurnSystem>();
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
        hitAdv = thisCard[0].hitAdv;
        blockAdv = thisCard[0].blockAdv;
        description = thisCard[0].description;

        thisSprite = thisCard[0].cardImage;

        nameText.text = "" + cardName;
        startUpText.text = "" + startUp + "f";
        recoveryText.text = "" + recovery + "f";
        descriptionText.text = description;
        damageText.text = "" + damage;
        rangeText.text = "" + range;
        hitAdvText.text = (hitAdv > 0) ? "+" + hitAdv : "" + hitAdv;
        blockAdvText.text = (blockAdv > 0) ? "+" + blockAdv : "" + blockAdv;

        thisImage.sprite = thisSprite;

        switch (thisCard[0].cardType) //Set the border color of card to match type of card
        {
            case "None": frameColor = Color.gray; break;
            case "Character": frameColor = Color.black; break;
            case "Attack": frameColor = Color.red; break;
            case "Block": frameColor = Color.blue; break;
            case "Grab": frameColor = Color.green; break;

            default: frameColor = Color.white; break;
        }
        frame.GetComponent<Image>().color = frameColor;

        //Sets the Card Styles based on the id given
        #region cardStyles
        if (thisCard[0].cardStyles.Length > 0)
        {
            switch (thisCard[0].cardStyles[0])
            {
                case 0: style1Image.sprite = Resources.Load<Sprite>("Images/StrikeIcon"); break;
                case 1: style1Image.sprite = Resources.Load<Sprite>("Images/BladeIcon"); break;
                default: style1Image.enabled = false; break;
            }
        }
        else
        {
            style1Image.enabled = false;
        }
        if (thisCard[0].cardStyles.Length > 1)
        {
            switch (thisCard[0].cardStyles[1])
            {
                case 0: style2Image.sprite = Resources.Load<Sprite>("Images/StrikeIcon"); break;
                case 1: style2Image.sprite = Resources.Load<Sprite>("Images/BladeIcon"); break;
                default: style2Image.enabled = false; break;
            }
        }
        else
        {
            style2Image.enabled = false;
        }
        #endregion cardStyles 

        //Set visibility on attack stats for block
        if (thisCard[0].cardType == "Block")
        {
            attackStats.SetActive(false);
        }
        else
        {
            attackStats.SetActive(true);
        }

        cardBackScript.UpdateCard(cardBack);
        staticCardBack = cardBack;

        if(this.tag == "Clone")
        {
            if (thisCard[0].id >= 0)
            {
                thisCard[0] = PlayerDeck.staticDeck[numberOfCardsInDeck - 1];
                numberOfCardsInDeck -= 1;
                playerDeck.deckSize -= 1;
            }
            //cardBack = false;
            this.tag = "Untagged";
        }

        if (isInDiscard == false) //Check if card is allowed to be played
        {
            canBePlayed = true;
        }
        else canBePlayed = false;

        //Turn off dragging if card cant be played
        if (canBePlayed == false || turnSystem.isYourTurn == false) 
        {
            gameObject.GetComponent<Draggable>().enabled = false;
        }
        else gameObject.GetComponent<Draggable>().enabled = true;

        playZone = GameObject.Find("Play Panel");

        if(this.transform.parent == playZone.transform)
        {

        }
    }
    public void DiscardCard()
    {
        //Dont Discard block or grab
        if (thisCard[0].id <= 1) 
        {
            transform.SetParent(Hand.transform);
        }
        //put card in discard pile
        else
        {

            Discard = GameObject.Find("Discard Panel");
            this.transform.SetParent(Discard.transform);
            isInDiscard = true;
        }
    }
}
