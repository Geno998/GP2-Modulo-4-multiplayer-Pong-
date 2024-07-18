using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Globalization;

public class PongPlayer : NetworkBehaviour
{

    [SyncVar(hook = nameof(ClientHandlePlayerNameUpdated))]
    private string playerName;
    public string PlayerName { get { return playerName; } }


    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    private bool isPartyOwner = false;
    public bool IsPartyOwner { get { return isPartyOwner; } }


    private Color playerColour = new Color();
    public Color PlayerColour { get { return playerColour; } }

    public GameObject barObject;


    public event Action<int> ClientOnResourceUpdated;
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;
    public static event Action ClientOnInfoUpdated;

    #region Server
    public override void OnStartServer()
    {


        DontDestroyOnLoad(gameObject);
    }

    public override void OnStopServer()
    {

    }


    [Command]
    public void CmdStartGame()
    {
        if (!isPartyOwner) return;

        ((PongNetworkManager)NetworkManager.singleton).StartGame();
    }


    [Server]
    public void SetPlayerColour(Color newTeamColour)
    {
        playerColour = newTeamColour;
    }

    [Server]
    public void SetIsPartyOwner(bool value)
    {
        isPartyOwner = value;
    }

    [Server]
    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }

    #endregion

    #region Client

    private void Update()
    {
        if (barObject == null)
        {
            setPlayerBar();
        }
    }


    public override void OnStartClient()
    {
        if (NetworkServer.active) return;

        ((PongNetworkManager)NetworkManager.singleton).Players.Add(this);
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStopClient()
    {
        ClientOnInfoUpdated?.Invoke();

        if (!isClientOnly) return;
        ((PongNetworkManager)NetworkManager.singleton).Players.Remove(this);
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!isOwned) return;

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }

    private void ClientHandleResourcesUpdated(int oldResources, int newResources)
    {
        ClientOnResourceUpdated?.Invoke(newResources);
    }

    private void ClientHandlePlayerNameUpdated(string oldName, string newName)
    {
        ClientOnInfoUpdated?.Invoke();
    }

    public void setPlayerBar()
    {
        if (isOwned)
        {
            foreach (PlayerBar foundBar in FindObjectsOfType<PlayerBar>())
            {
                if (foundBar.isOwned)
                {
                    barObject = foundBar.gameObject;
                }
            }
        }
        else
        {
            foreach (PlayerBar foundBar in FindObjectsOfType<PlayerBar>())
            {
                if (!foundBar.isOwned)
                {
                    barObject = foundBar.gameObject;
                }
            }
        }
    }

    #endregion
}
