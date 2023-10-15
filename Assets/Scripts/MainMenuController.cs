using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Gameplay";
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
