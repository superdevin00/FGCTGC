using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystem : MonoBehaviour
{
    public bool isYourTurn;
    public int yourTurn;
    public int yourOpponentTurn;
    public TMP_Text turnText;
    public GameObject playZone;

    public OpponentDeck oppDeck;
    public PlayerDeck playerDeck;

    public static bool startTurn;

    // Start is called before the first frame update
    void Start()
    {
        isYourTurn = true;
        yourTurn = 1;
        yourOpponentTurn = 0;
        playZone = GameObject.Find("Play Panel");
        oppDeck = GameObject.Find("Opponent Deck Panel").GetComponent<OpponentDeck>();

        startTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isYourTurn)
        {
            turnText.text = "Your Turn";

        }else turnText.text = "Opponent Turn";
    }

    public void EndYourTurn()
    {
        isYourTurn = false;
        yourOpponentTurn += 1;
        if (playZone.transform.childCount != 1)
        {
            Debug.Log("Too Many cards in playzone");
        }

        //oppDeck.Draw(1);
        
    }
    public void EndOpponentTurn()
    {
        isYourTurn = true;
        yourTurn += 1;

        startTurn = true;
    }

    public void ReturnToNeutral()
    {
        oppDeck.Draw(1);
        playerDeck.Draw(1);
    }

    public void PlayerCanPlay(bool canPlay)
    {
        isYourTurn = canPlay;
    }
}
