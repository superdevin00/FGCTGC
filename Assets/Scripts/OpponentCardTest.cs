using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCardTest : MonoBehaviour
{
    public Card opponentCard;
    public int x;
    // Start is called before the first frame update
    void Start()
    {
        x = Random.Range(1, 4);
        opponentCard = CardDataBase.cardList[x];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
