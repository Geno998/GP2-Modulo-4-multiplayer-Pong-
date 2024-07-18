using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class MoveInput : MonoBehaviour
{

    [SerializeField] float speed;


    private PongPlayer player;
    [SerializeField] private PlayerBar bar;
    private Ball ball;

    private void OnEnable()
    {
        GameOverHandeler.ClientOnGameOver += ClientHandleGameOver;
    }

    private void OnDisable()
    {
        GameOverHandeler.ClientOnGameOver -= ClientHandleGameOver;
    }

    private void Start()
    {
        player = NetworkClient.connection.identity.GetComponent<PongPlayer>();
    }


    void Update()
    {
        if (bar == null)
        {
            bar = player.barObject.GetComponent<PlayerBar>();
        }

        if (ball == null)
        {
            ball = FindObjectOfType<Ball>();
        }


        if (Input.GetKey(KeyCode.Space) && ball.Standby)
        {
            if (ball.isOwned)
            {
                ball.CmdLaunchBall();
            }
        }

        if (Input.GetKey(KeyCode.R) && !ball.Standby)
        {
            if (ball.isOwned)
            {
                ball.ResetBall();
            }

        }


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) return;


            if (Input.GetKey(KeyCode.W))
            {
                bar.CmdMove(speed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                bar.CmdMove(-speed);
            }
        }
    }

    private void ClientHandleGameOver(int winnerIP)
    {
        enabled = false;
    }
}

