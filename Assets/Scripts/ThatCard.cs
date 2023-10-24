using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ThatCard : MonoBehaviour
{
    public List<Card> thatCard = new List<Card>();
    public int thatId;

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

    public Sprite thatSprite;
    public Image thatImage;
    public Image frame;
    private Color frameColor;

    public bool cardBack;
    public static bool staticCardBack;
    CardBack cardBackScript;

    public GameObject Hand;

    public OpponentDeck opponentDeck;
    public int numberOfCardsInDeck;

    public GameObject playZone;

    public bool canBePlayed;

    public GameObject Discard;
    public bool isInDiscard;

    // Start is called before the first frame update
    void Start()
    {
        opponentDeck = GameObject.Find("Opponent Deck Panel").GetComponent<OpponentDeck>();
        //thatCard[0] = CardDataBase.cardList[thatId];
        numberOfCardsInDeck = opponentDeck.deckSize;
    }

    // Update is called once per frame
    void Update()
    {
        Hand = GameObject.Find("Opponent Card Panel");
        if (this.transform.parent == Hand.transform)
        {
            cardBack = false;

        }

        cardBackScript = GetComponent<CardBack>();
        id = thatCard[0].id;
        cardName = thatCard[0].cardName;
        cardStyles = thatCard[0].cardStyles;
        startUp = thatCard[0].startUp;
        recovery = thatCard[0].recovery;
        damage = thatCard[0].damage;
        range = thatCard[0].range;
        hitAdv = thatCard[0].hitAdv;
        blockAdv = thatCard[0].blockAdv;
        description = thatCard[0].description;

        thatSprite = thatCard[0].cardImage;

        nameText.text = "" + cardName;
        startUpText.text = "" + startUp + "f";
        recoveryText.text = "" + recovery + "f";
        descriptionText.text = description;
        damageText.text = "" + damage;
        rangeText.text = "" + range;
        hitAdvText.text = (hitAdv > 0) ? "+" + hitAdv : "" + hitAdv;
        blockAdvText.text = (blockAdv > 0) ? "+" + blockAdv : "" + blockAdv;

        thatImage.sprite = thatSprite;

        switch (thatCard[0].cardType)
        {
            case "None": frameColor = Color.gray; break;
            case "Character": frameColor = Color.black; break;
            case "Attack": frameColor = Color.red; break;
            case "Block": frameColor = Color.blue; break;
            case "Grab": frameColor = Color.green; break;
            case "Special": frameColor = Color.magenta; break;

            default: frameColor = Color.white; break;
        }
        frame.GetComponent<Image>().color = frameColor;

        //Sets the Card Styles based on the id given
        #region cardStyles
        if (thatCard[0].cardStyles.Length > 0)
        {
            switch (thatCard[0].cardStyles[0])
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
        if (thatCard[0].cardStyles.Length > 1)
        {
            switch (thatCard[0].cardStyles[1])
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

        cardBackScript.UpdateCard(cardBack);
        staticCardBack = cardBack;

        if (this.tag == "Clone")
        {
            //thatCard[0];
            numberOfCardsInDeck -= 1;
            opponentDeck.deckSize -= 1;
            //cardBack = false;
            this.tag = "Untagged";
        }

        if (isInDiscard == false) //Check if card is allowed to be played
        {
            canBePlayed = true;
        }
        else canBePlayed = false;

        /*if (canBePlayed == false) //Turn off dragging if card cant be played
        {
            gameObject.GetComponent<Draggable>().enabled = false;
        }
        else gameObject.GetComponent<Draggable>().enabled = true;*/

        playZone = GameObject.Find("Play Panel");

        if (this.transform.parent == playZone.transform)
        {

        }
    }
    public void DiscardCard()
    {
        Destroy(gameObject);
    }
}
