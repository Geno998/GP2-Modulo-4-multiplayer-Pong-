using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using Unity.VisualScripting;

public class Points : NetworkBehaviour
{
    [SerializeField] private int startPoints = 0;

    public event Action ServerOnGameEnded;
    public event Action<int, int> ClientOnPointsUpdated;
    public static event Action<PlayerBar> ServerOnPointsReached;

    [SyncVar(hook = nameof(HandleOnHealthUpdated))]
    [SerializeField] private int points;

    public int currentPoints { get { return points; } }

    #region Server
    public override void OnStartServer()
    {
        points = 0;


        PlayerBar.ServerOGameFinished += ServerHandleGameFinished;
    }

    public override void OnStopServer()
    {
        PlayerBar.ServerOGameFinished -= ServerHandleGameFinished;
    }

    [Command]
    public void GivePoints()
    {

        if (points != 5)
        {
            points = Mathf.Max(points + 1, 0);
        }


        if (points == 5)
        {
            ServerOnPointsReached?.Invoke(gameObject.GetComponent<PlayerBar>());
            ServerOnGameEnded?.Invoke();
        }


    }

    [Server]
    private void ServerHandleGameFinished(int connectionId)
    {
        if (connectionToClient.connectionId != connectionId) return;
    }

    #endregion

    #region Client

    private void HandleOnHealthUpdated(int oldPoint, int newPoint)
    {
        ClientOnPointsUpdated?.Invoke(newPoint, startPoints);
    }

    #endregion
}
