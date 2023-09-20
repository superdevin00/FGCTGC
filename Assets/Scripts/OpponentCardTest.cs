using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCardTest : MonoBehaviour
{
    public Card opponentCard;
    public int x;
    public GameObject It;
    public GameObject OpponentPlay;
    // Start is called before the first frame update
    void Start()
    {
        OpponentPlay = GameObject.Find("Opponent Card Panel");
        It.transform.SetParent(OpponentPlay.transform);
        It.transform.localScale = Vector3.one;
        It.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
        It.transform.eulerAngles = new Vector3(0, 0, 0);

        //x = Random.Range(1, 4);
        //opponentCard = CardDataBase.cardList[x];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
