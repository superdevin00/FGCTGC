using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCollection : MonoBehaviour
{
    public GameObject deckBuilderCard;
    private GameObject tempCard;
    private int currentPage;
    private int cardsOnPage;
    private int totalPages;

    public GameObject prevPageButton;
    public GameObject nextPageButton;

    // Start is called before the first frame update
    void Start()
    {
        currentPage = 1;
        GetTotalPages();
        PopulateCardCollection();
        UpdatePageButtons();
    }

    private void PopulateCardCollection()
    {
        for (int i = 0; i < CheckCardsOnPage(currentPage); i++)
        {
            tempCard = Instantiate(deckBuilderCard, transform.position, transform.rotation);
            tempCard.GetComponent<ThisCard>().thisId = ((i+2) + ((currentPage - 1) * 16));
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

    private void GetTotalPages()
    {
        Debug.Log((int)Mathf.Ceil(((float)CardDataBase.cardList.Count - 2) / 16));
        totalPages = (int)Mathf.Ceil(((float)CardDataBase.cardList.Count - 2)/16);
    }

    private int CheckCardsOnPage(int page)
    {
        if (page < totalPages)
        {
            //If not last page
            cardsOnPage = 16;
        }
        else
        {
            //Check cards on last page
            cardsOnPage = (CardDataBase.cardList.Count - 2) % 16;
            if (cardsOnPage == 0) { cardsOnPage = 16; }
        }
        return cardsOnPage;
    }

    private void UpdatePageButtons()
    {
        //Hide Previous Page Button if on first page
        if (currentPage <= 1)
        {
            prevPageButton.SetActive(false);
        }
        else
        {
            prevPageButton.SetActive(true);
        }

        //Hide Next Page button if on last page
        if (currentPage >= totalPages)
        {
            nextPageButton.SetActive(false);
        }
        else
        {
            nextPageButton.SetActive(true);
        }
    }

    public void NextPage()
    {
        //Check if not last page
        if (currentPage < totalPages)
        {
            currentPage++;
        }

        //Refresh collection
        UpdatePageButtons();
        ClearCardCollection();
        PopulateCardCollection();
    }

    public void PrevioustPage()
    {
        //Check if not first page
        if (currentPage > 1)
        {
            currentPage--;
        }

        //Refresh collection
        UpdatePageButtons();
        ClearCardCollection();
        PopulateCardCollection();
    }

    private void Update()
    {
        if (transform.childCount != CheckCardsOnPage(currentPage) && Input.GetMouseButton(0) == false)
        {
            Debug.Log("Call Clear");
            ClearCardCollection();
            PopulateCardCollection();
        }

    }
}
