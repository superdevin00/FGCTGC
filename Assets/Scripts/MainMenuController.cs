using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Gameplay";
    public TMP_Text joinErrorText;
    public TMP_InputField ipInput;
    //private string localHostStandin;


    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void TryJoin()
    {
        if(ipInput.text == "localhost")
        {
            joinErrorText.text = "Joining Local Host";
            joinErrorText.gameObject.SetActive(true);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", (ushort)7777);
            StartClient();
        }
        else if (ipInput.text.Length >= 7 && ipInput.text.Length <= 15)
        {
            joinErrorText.text = "Joining Match";
            joinErrorText.gameObject.SetActive(true);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipInput.text, (ushort)7777);
            StartClient();
        }
        else
        {
            joinErrorText.text = "Invalid IP";
            joinErrorText.gameObject.SetActive(true);
        }
    }

    public void GoToDeckBuilder()
    {
        SceneManager.LoadScene("DeckBuilder");
    }
}
