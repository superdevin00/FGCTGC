using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BayatGames.SaveGameFree;
using UnityEngine.SceneManagement;
public class DeckBuilder : MonoBehaviour
{
    private TMP_Text deckCountText;
    private TMP_Text errorMessage;
    private int deckCount;
    public List<int> deck = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        deckCount = 0;
        deckCountText = GameObject.Find("Deck Count").GetComponent<TMP_Text>();
        errorMessage = GameObject.Find("Error Message").GetComponent<TMP_Text>();
        errorMessage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        deckCount = gameObject.transform.childCount;

        deckCountText.text = deckCount + "/20";
    }

    public void SaveDeck()
    {
        if (deckCount != 20)
        {
            errorMessage.text = "Invalid Deck Size";
            errorMessage.gameObject.SetActive(true);
        }
        else
        {

            for (int i = 0; i < deckCount; i++)
            {
                Debug.Log("Try Save");
                /*Debug.Log(gameObject.transform.GetChild(i).GetComponent<ThisCard>().id);
                Debug.Log(i);*/
                deck.Add(gameObject.transform.GetChild(i).GetComponent<ThisCard>().id);
            }
            SaveGame.DeleteAll();
            SaveGame.Save("Deck", deck);
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
