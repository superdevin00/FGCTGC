using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardToHand : MonoBehaviour
{
    public GameObject Hand;
    public GameObject It;
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("Hand Panel") != null)
        {
            Hand = GameObject.Find("Hand Panel");
            It.transform.SetParent(Hand.transform);
            It.transform.localScale = Vector3.one;
            It.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
            It.transform.eulerAngles = new Vector3(0, 0, 0);

        }
        else if(GameObject.Find("Card Collection") != null)
        {
            Hand = GameObject.Find("Card Collection");
            It.transform.SetParent(Hand.transform);
            //It.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            It.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
            It.transform.eulerAngles = new Vector3(0, 0, 0);
            It.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
