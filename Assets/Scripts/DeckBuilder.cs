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
            try
            {
                SaveGame.Delete("Deck");
                Debug.Log(SaveGame.SavePath.ToString());
                for (int i = 0; i < deckCount; i++)
                {
                    Debug.Log("Try Save");
                    /*Debug.Log(gameObject.transform.GetChild(i).GetComponent<ThisCard>().id);
                    Debug.Log(i);*/
                    deck.Add(gameObject.transform.GetChild(i).GetComponent<ThisCard>().id);
                }
                SaveGame.Delete("Deck");
                SaveGame.Save("Deck", deck);
                errorMessage.text = "Deck Saved!";
                errorMessage.gameObject.SetActive(true);
            }
            catch
            {
                errorMessage.text = "An error has occured";
                errorMessage.gameObject.SetActive(true);
            }
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
