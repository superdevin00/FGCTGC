using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    private void Awake()
    {
        // ID, Name, Styles{styleX,styleY}, StartUp, Recovery, Damage, Range, Descrition, Card Sprite, Card Type
        cardList.Add(new Card(0, "Test", new int[2] { 0, 1 }, 1, 2,5,0,"Fortnite", Resources.Load<Sprite>("Images/NoImg"), "None"));
        cardList.Add(new Card(1, "Quick Jab", new int[1] { 0 }, 3,5,5,0,"", Resources.Load<Sprite>("Images/StrikeIcon"), "Attack"));
        cardList.Add(new Card(2, "Palm Strike", new int[1] { 0 }, 3, 12, 12, 0, "Push 1", Resources.Load<Sprite>("Images/StrikeIcon"), "Attack"));
        cardList.Add(new Card(3, "Roundhouse", new int[1] { 0 }, 6, 10, 15, 1, "", Resources.Load<Sprite>("Images/StrikeIcon"), "Attack"));
        cardList.Add(new Card(4, "Leg Sweep", new int[1] { 0 }, 7, 10, 16, 1, "Knockdown", Resources.Load<Sprite>("Images/StrikeIcon"), "Attack"));
        cardList.Add(new Card(5, "Solar Plexus Strike", new int[1] {0}, 6,13,8,0,"F5: Step In", Resources.Load<Sprite>("Images/StrikeIcon"), "Attack"));
    }
}
