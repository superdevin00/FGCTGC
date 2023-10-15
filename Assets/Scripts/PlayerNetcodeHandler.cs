using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerNetcodeHandler : NetworkBehaviour
{
    private GameController gameController;

    private NetworkObject player;
    //private PlayerData playerData;
    //private ulong playerID;
    public NetworkObject opponent;
    private PlayerNetcodeHandler opponentData;
    private ulong testClientId;
    

    public NetworkVariable<bool> turnReady = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> cardPlayedID = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> canPlayCard = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isCheckingWinner = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> checkWinnerStep = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {   
        //Check if opponent is ready
        /*opponentData.turnReady.OnValueChanged += (bool oldval, bool newval) =>
        {
            SetOpponentReady(newval);
        };
        //Check opponents played card
        opponentData.cardPlayedID.OnValueChanged += (int oldval, int newval) =>
        {
            SetOpponentCardId(newval);
        };*/
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            turnReady.Value = false;
            cardPlayedID.Value = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Dont run code if not owner
        if (!IsOwner) return;

        //Get Player
        player = NetworkManager.LocalClient.PlayerObject;


        //Get Game Controller Object
        if (SceneManager.GetActiveScene().name == "Gameplay" && gameController == null)
        {
            gameController = GameObject.Find("GameController").GetComponent<GameController>();
        }

        //Check if opponent is connected
        if (gameController != null)
        {
            if (opponent == null)
            {
                FindOpponent();
            }
            else
            {
                opponentData = opponent.GetComponent<PlayerNetcodeHandler>();
                SetOpponentReady(opponentData.turnReady.Value);
                SetCanOpponentPlayCard(opponentData.canPlayCard.Value);
                SetOpponentIsCheckingWinner(opponentData.isCheckingWinner.Value);
                SetOpponentCheckWinnerStep(opponentData.checkWinnerStep.Value);
            }
        }

        if (opponent != null)
        {
            canPlayCard.Value = (gameController.playerCard != null)? true : false;
            isCheckingWinner.Value = gameController.isCheckingWinner;
            checkWinnerStep.Value = gameController.checkWinnerStep;

            if (IsClient)
            {
                gameController.isWaitingForOpponent = false;
            }
            //Check if player is ready
            if (gameController.playerReady == true)
            {
                turnReady.Value = true;
                if (gameController.playerCard != null)
                {
                    cardPlayedID.Value = gameController.playerCard.id;
                }
                else
                {
                    cardPlayedID.Value = -1;
                }
            }
            else
            {
                turnReady.Value = false;
                //cardPlayedID.Value = -1;
            }

        }
    }

    public void FindOpponent()
    {
        foreach (GameObject testObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (testObj.GetInstanceID() != player.gameObject.GetInstanceID())
            {
                opponent = testObj.GetComponent<NetworkObject>();
            }
        }
    }

    public void SetOpponentReady(bool isReady)
    {
        gameController.opponentReady = isReady;
        SetOpponentCardId(opponentData.cardPlayedID.Value);
    }
    public void SetOpponentCardId(int cardId)
    {
        gameController.opponentCardID = cardId;
    }
    public void SetCanOpponentPlayCard(bool canPlayCard)
    {
        gameController.canOpponentPlayCard=canPlayCard;
    }
    public void SetOpponentIsCheckingWinner(bool isChecking)
    {
        gameController.isOpponentCheckingWinner = isChecking;
    }
    public void SetOpponentCheckWinnerStep(int step)
    {
        gameController.opponentCheckWinnerStep = step;
    }
}
