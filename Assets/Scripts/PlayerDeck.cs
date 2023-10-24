using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

public class PlayerDeck : MonoBehaviour
{

    public List<Card> deck = new List<Card>();
    public Card container;
    public static List<Card> staticDeck = new List<Card>();

    public int x;
    public int deckSize;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;

    public GameObject cardToHand;
    public GameObject cardBack;
    public GameObject Deck;

    public GameObject[] Clones;

    public GameObject Hand;

    // Start is called before the first frame update
    void Start()
    { 
        x = 0;
        deckSize = 22;

        LoadPlayerDeck();

        StartCoroutine(StartGame());
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

        if(TurnSystem.startTurn == true) //At the beginning of player turn
        {
            Draw(1); //Draw 1 card
            TurnSystem.startTurn = false;
        }
    }

    private void LoadPlayerDeck()
    {
        //Load Deck
        deck = SaveGame.Load<List<Card>>("Deck");

        //Randomize if no deck set
        if (deck == null)
        {
            Debug.Log("DeckRandom");
            for (int i = 0; i < deckSize - 2; i++)
            {
                x = Random.Range(2, 7);
                deck.Add(CardDataBase.cardList[x]);
            }
            
        }
        else
        {
            Debug.Log("NotRandom");
        }

        Debug.Log(deck.Count);
        //Shuffle
        Shuffle();

        //Add Block and Grab
        deck.Add(CardDataBase.cardList[0]);
        deck.Add(CardDataBase.cardList[1]);
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(1);
        Clones = GameObject.FindGameObjectsWithTag("Clone");

        foreach(GameObject Clone in Clones)
        {
            Destroy(Clone);
        }
    }

    IEnumerator StartGame()
    {
        for (int i =0;i<7;i++)
        {
            yield return new WaitForSeconds(0.2f);
            Instantiate(cardToHand, transform.position, transform.rotation);
        }
        
    }

    public void Shuffle()
    {
        Debug.Log("StartShuffle");
        for (int i = 0; i < deck.Count - 1; i++)
        {
            Debug.Log("ShuffleStep");
            container = deck[i];
            int randomIndex = Random.Range(i, deck.Count - 1);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container;
            
        }
        //Instantiate(cardBack, transform.position, transform.rotation);
        //StartCoroutine(Example());
    }

    public void Draw(int x)
    {
        StartCoroutine(DrawRoutine(x));
    }

    public void DrawTo(int x)
    {
        Debug.Log(Hand.transform.childCount);
        if (Hand.transform.childCount < x)
        {
            StartCoroutine(DrawRoutine(x - Hand.transform.childCount));
        }
    }

    IEnumerator DrawRoutine(int x)
    {
        for(int i=0; i<x; i++)
        {
            yield return new WaitForSeconds(1);
            Instantiate(cardToHand, transform.position, transform.rotation);
        }
    }
}
