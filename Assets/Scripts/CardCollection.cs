using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCollection : MonoBehaviour
{
    public GameObject deckBuilderCard;
    private GameObject tempCard;

    // Start is called before the first frame update
    void Start()
    {
        PopulateCardCollection();
    }

    private void PopulateCardCollection()
    {
        for (int i = 0; i < CardDataBase.cardList.Count; i++)
        {
            tempCard = Instantiate(deckBuilderCard, transform.position, transform.rotation);
            tempCard.GetComponent<ThisCard>().thisId = i;
        }

        //gameObject.transform.parent.transform.localScale = gameObject.GetComponent<GridLayoutGroup>().;
    }

    private void ClearCardCollection()
    {
        foreach (Transform child in gameObject.transform.GetComponentInChildren<Transform>())
        {
            if (child.GetComponent<ThisCard>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void Update()
    {
        if (transform.childCount != CardDataBase.cardList.Count && Input.GetMouseButton(0) == false)
        {
            Debug.Log("Call Clear");
            ClearCardCollection();
            PopulateCardCollection();
        }

    }
}
