using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Mirror;
public class GameOverHandeler : NetworkBehaviour
{


    public static event Action ServerOnGameOver;
    public static event Action<int> ClientOnGameOver;

    #region Server

    public override void OnStartServer()
    {

        Points.ServerOnPointsReached += ServerHandlePlayerDespawned;
    }


    public override void OnStopServer()
    {

        Points.ServerOnPointsReached -= ServerHandlePlayerDespawned;
    }

    [Server]
    private void ServerHandlePlayerDespawned(PlayerBar playerBar)
    {
        int playerId = playerBar.connectionToClient.connectionId;

        GameOver(playerId);
        ServerOnGameOver?.Invoke();
    }


    #endregion

    #region Client

    [ClientRpc]
    private void GameOver(int winner)
    {
        ClientOnGameOver?.Invoke(winner);
    }

    #endregion
}
