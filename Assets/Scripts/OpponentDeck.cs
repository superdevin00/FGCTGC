﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentDeck : MonoBehaviour
{

    public List<Card> deck = new List<Card>();
    public List<Card> container = new List<Card>();
    public static List<Card> staticDeck = new List<Card>();

    public int x;
    public int deckSize;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;

    public GameObject cardToPlay;
    public GameObject cardBack;
    public GameObject Deck;

    public GameObject[] Clones;

    public GameObject Hand;

    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        deckSize = 20;

        for (int i = 0; i < deckSize; i++)
        {
            deck[i] = CardDataBase.cardList[0];
        }
        //StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        staticDeck = deck;

        if (deckSize < 15)
        {
            cardInDeck1.SetActive(false);
        }
        if (deckSize < 10)
        {
            cardInDeck2.SetActive(false);
        }
        if (deckSize < 5)
        {
            cardInDeck3.SetActive(false);
        }
        if (deckSize <= 0)
        {
            cardInDeck4.SetActive(false);
        }

        /*if (TurnSystem.startTurn == true) //At the beginning of player turn
        {
            StartCoroutine(Draw(1)); //Draw 1 card
            TurnSystem.startTurn = false;
        }*/
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(1);
        Clones = GameObject.FindGameObjectsWithTag("Clone");

        foreach (GameObject Clone in Clones)
        {
            Destroy(Clone);
        }
    }

    IEnumerator StartGame()
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(1);
            Instantiate(cardToPlay, transform.position, transform.rotation);
        }
    }

    public void Shuffle()
    {
        for (int i = 0; i < deckSize; i++)
        {
            container[0] = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container[0];
        }
        Instantiate(cardBack, transform.position, transform.rotation);
        StartCoroutine(Example());
    }

    public void Draw(int x)
    {
        StartCoroutine(DrawRoutine(x));
    }

    IEnumerator DrawRoutine(int x)
    {
        for (int i = 0; i < x; i++)
        {
            yield return new WaitForSeconds(1);
            Instantiate(cardToPlay, transform.position, transform.rotation);
        }
    }

    public void playOpponentCard(int cardId)
    {
        GameObject oppCard = Instantiate(cardToPlay, transform.position, transform.rotation);
        oppCard.GetComponent<ThatCard>().thatId = cardId;
        oppCard.GetComponent<ThatCard>().thatCard[0] = CardDataBase.cardList[cardId];
    }



}
