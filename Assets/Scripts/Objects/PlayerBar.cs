using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using Unity.VisualScripting;

public class PlayerBar : NetworkBehaviour
{

    [SerializeField] private Points points;

    public static event Action<int> ServerOGameFinished;
    public static event Action<PlayerBar> ServerOnPointsReached;

    #region Server

    public override void OnStartServer()
    {
        points.ServerOnGameEnded += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        points.ServerOnGameEnded -= ServerHandleDie;
    }

    [Server]
    public void ServerOnGameFinished()
    {
        if (points.currentPoints == 5)
        {
            ServerOnPointsReached?.Invoke(this);
        }
    }


    [Server]
    private void ServerHandleDie()
    {
        ServerOGameFinished?.Invoke(connectionToClient.connectionId);
    }

    [Command]
    public void CmdMove(float speed)
    {
        if (speed > 0)
        {
            if (transform.position.y > 3.45) return;
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
        else if (speed < 0)
        {
            if (transform.position.y < -3.45) return;
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
    }


    #endregion

    #region Client

    #endregion
}
