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

    // Start is called before the first frame update
    void Start()
    {
        isYourTurn = true;
        yourTurn = 1;
        yourOpponentTurn = 0;
        playZone = GameObject.Find("Play Panel");


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
        
    }
    public void EndOpponentTurn()
    {
        isYourTurn = true;
        yourTurn += 1;
    }
}
