using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();
    //public static List<Card> staticCardList = new List<Card>();

    private void Awake()
    {
        // ID, Name, Styles{styleX,styleY}, StartUp, Recovery, Damage, Range, Hit Adv, Block Adv, Descrition, Card Sprite, Card Type
        cardList.Add(new Card(0, "Block", new int[0] {}, 1, 10, 0, 0, 1, 0, "Defend Against Incoming Damage", Resources.Load<Sprite>("Images/NoImg"), "Block"));
        cardList.Add(new Card(1, "Grab", new int[0] {}, 5, 10, 5, 0, 1, 0, "Knockdown", Resources.Load<Sprite>("Images/NoImg"), "Grab"));

        //cardList.Add(new Card(0, "Test", new int[2] { 0, 1 }, 1, 2,5,0,1,0,"Fortnite", Resources.Load<Sprite>("Images/NoImg"), "None"));
        cardList.Add(new Card(2, "Quick Jab", new int[1] { 0 }, 3,5,5,0,1,0,"", Resources.Load<Sprite>("Images/StrikeIcon"), "Attack"));
        cardList.Add(new Card(3, "Palm Strike", new int[1] { 0 }, 3, 12, 12, 0,6,1, "Push 1", Resources.Load<Sprite>("Images/StrikeIcon"), "Attack"));
        cardList.Add(new Card(4, "Roundhouse", new int[1] { 0 }, 6, 10, 15, 1,5,-1, "", Resources.Load<Sprite>("Images/StrikeIcon"), "Attack"));
        cardList.Add(new Card(5, "Leg Sweep", new int[1] { 0 }, 7, 10, 16, 1,6,-5, "Knockdown", Resources.Load<Sprite>("Images/StrikeIcon"), "Attack"));
        cardList.Add(new Card(6, "Solar Plexus Strike", new int[1] {0}, 8,13,8,0,5,2,"F5: Step In", Resources.Load<Sprite>("Images/StrikeIcon"), "Attack"));
    }
}
