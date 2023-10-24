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
    public NetworkVariable<int> turnSync = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> neutralSync = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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
                SetOpponentNetVar(opponentData);
            }
        }

        if (opponent != null)
        {
            GetPlayerNetVar();

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

    /*public void SetOpponentReady(bool isReady)
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
    public void SetOpponentTurnSync(int turn)
    {
        gameController.opponentTurnSync = turn;
    }
    public void SetOpponentNeutralSync(int neutral)
    {
        gameController.opponentNeutralSync = neutral;
    }*/

    public void GetPlayerNetVar()
    {
        canPlayCard.Value = (gameController.playerCard != null) ? true : false;
        isCheckingWinner.Value = gameController.isCheckingWinner;
        checkWinnerStep.Value = gameController.checkWinnerStep;
        turnSync.Value = gameController.playerTurnSync;
        neutralSync.Value = gameController.playerNeutralSync;
    }
    public void SetOpponentNetVar(PlayerNetcodeHandler oppData)
    {
        gameController.opponentReady = oppData.turnReady.Value;
        gameController.opponentCardID = oppData.cardPlayedID.Value;
        gameController.canOpponentPlayCard = oppData.canPlayCard.Value;
        gameController.isOpponentCheckingWinner = oppData.isCheckingWinner.Value;
        gameController.opponentCheckWinnerStep = oppData.checkWinnerStep.Value;
        gameController.opponentTurnSync = oppData.turnSync.Value;
        gameController.opponentNeutralSync = oppData.neutralSync.Value;
    }
}
