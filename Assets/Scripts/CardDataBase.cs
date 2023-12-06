using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();
    //public static List<Card> staticCardList = new List<Card>();

    private void Awake()
    {
        //Flushes the cardlist so it doesnt grow on reloads
        cardList.Clear();

        // ID, Name, Styles{styleX,styleY}, StartUp, Recovery, Damage, Range, Hit Adv, Block Adv, Descrition, Card Sprite, Card Type
        cardList.Add(new Card(0, "Block", new int[0] {}, 1, 10, 0, 0, 1, 0, "Defend Against Incoming Damage", Resources.Load<Sprite>("Images/NoImg"), "Block"));
        cardList.Add(new Card(1, "Grab", new int[0] {}, 5, 10, 5, 0, 1, 0, "Knockdown", Resources.Load<Sprite>("Images/NoImg"), "Grab"));

        cardList.Add(new Card(2, "Quick Jab", new int[1] { 0 }, 3,5,5,0,1,0,"", Resources.Load<Sprite>("Images/QuickJabProto"), "Attack"));
        cardList.Add(new Card(3, "Palm Strike", new int[1] { 0 }, 6, 12, 12, 0,6,1, "Push 1", Resources.Load<Sprite>("Images/PalmStrikeProto"), "Attack"));
        cardList.Add(new Card(4, "Roundhouse", new int[1] { 0 }, 6, 14, 15, 2,5,-1, "", Resources.Load<Sprite>("Images/Roundhouse"), "Attack"));
        cardList.Add(new Card(5, "Leg Sweep", new int[1] { 0 }, 7, 10, 16, 1,6,-5, "Knockdown", Resources.Load<Sprite>("Images/LegSweepProto"), "Attack"));
        cardList.Add(new Card(6, "Solar Plexus Strike", new int[1] {0}, 8,13,8,0,5,2,"F5: Step In", Resources.Load<Sprite>("Images/SolarPlexusStrike"), "Attack"));
        cardList.Add(new Card(7, "Footsies", new int[1] { 0 }, 5, 10, 8, 2, 6, 2, "", Resources.Load<Sprite>("Images/Footsies"), "Attack"));
        cardList.Add(new Card(8, "Flicker Jabs", new int[1] { 0 }, 5, 12, 6, 0, 1, -2, "On Hit: 50% chance to hit again.", Resources.Load<Sprite>("Images/FlickerJab"), "Special"));
        cardList.Add(new Card(9, "Gutpunch", new int[1] { 0 }, 5, 10, 8, 0, 3, -2, "Special Cancel: +8", Resources.Load<Sprite>("Images/GutPunch"), "Attack")); //WIP
        cardList.Add(new Card(10, "Quick Step", new int[1] { 0 }, 1, 8, 0, 0, 0, 0, "F1: Step In\nF:5 Step In", Resources.Load<Sprite>("Images/QuickStep"), "Attack"));
        cardList.Add(new Card(11, "Dash Punch", new int[1] { 0 }, 7, 12, 12, 0, 5, -5, "F5: Step In 2", Resources.Load<Sprite>("Images/DashPunch"), "Special")); 
        cardList.Add(new Card(12, "Swaying Strike", new int[1] { 0 }, 9, 15, 10, 0, 7, 2, "F3: Step Out\nF7: Step In", Resources.Load<Sprite>("Images/StrikeIcon"), "Special"));
        cardList.Add(new Card(13, "Collar Bone Breaker", new int[1] { 0 }, 7, 13, 10, 0, 1, 8, "", Resources.Load<Sprite>("Images/CollarBoneBreaker"), "Attack"));
        cardList.Add(new Card(14, "Flying Dropkick", new int[1] { 0 }, 11, 18, 18, 1, 4, -4, "F1: Step In\nF7: Step In\nKnockdown", Resources.Load<Sprite>("Images/StrikeIcon"), "Special"));
        cardList.Add(new Card(15, "Hilt Strike", new int[1] { 1 }, 4, 7, 6, 1, 1, -1, "", Resources.Load<Sprite>("Images/BladeIcon"), "Attack"));
        cardList.Add(new Card(16, "Grapple Hook", new int[1] { 1 }, 12, 20, 10, 5, 4, -6, "On Hit: Pull 2\nOn Block: Pull 1", Resources.Load<Sprite>("Images/GrappleHook"), "Attack"));
        cardList.Add(new Card(17, "Kunai & Chain", new int[1] { 1 }, 8, 16, 14, 3, 3, -5, "On Hit: Push 2\nOn Block: Push 1", Resources.Load<Sprite>("Images/KunaiAndChain"), "Attack"));
        cardList.Add(new Card(18, "Quick Chop", new int[1] { 2 }, 5, 8, 10, 1, 4, 1, "", Resources.Load<Sprite>("Images/GrappleIcon"), "Attack"));
        cardList.Add(new Card(19, "Steady Fist", new int[1] { 3 }, 4, 7, 6, 0, 1, 1, "Special Cancel: +4", Resources.Load<Sprite>("Images/YangKiIcon"), "Attack"));//WIP
        cardList.Add(new Card(20, "Claw Swipe", new int[1] { 4 }, 3, 7, 8, 0, 4, -3, "", Resources.Load<Sprite>("Images/YinKiIcon"), "Attack"));

        //cardList.Add(new Card(18, "Flicker Jabs", new int[1] { 0 }, 5, 12, 6, 0, 1, -2, "On Hit: 50% chance to hit again.", Resources.Load<Sprite>("Images/StrikeIcon"), "Special"));

        cardList.Add(new Card(21, "Vicious Mockery", new int[0] {}, 10, 15, 2, 6, 0, 0, "\"Cause a bit of mental damage by shouting some rather rude words\"", Resources.Load<Sprite>("Images/MagicIcon"), "Special"));
    }
}
