using Mirror;
using Org.BouncyCastle.Asn1.Cmp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : NetworkBehaviour
{
    Ball ball;
    PongPlayer player;
    GameObject bar;


    private void Start()
    {
        player = NetworkClient.connection.identity.GetComponent<PongPlayer>();
        ball = FindObjectOfType<Ball>();
    }

    [ClientRpc]

    public void RpcGoal()
    {

        if (isOwned)
        {
            player.barObject.GetComponent<Points>().GivePoints();
        }
    }

    [Server]
    public void ServerBallReset()
    {
        ball.ResetBall();
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        RpcGoal();
        ServerBallReset();
    }
}
