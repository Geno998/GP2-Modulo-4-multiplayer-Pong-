using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using Org.BouncyCastle.Asn1.Esf;

public class Ball : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] int force = 10;


    private bool standby = true;
    public bool Standby { get { return standby; } }



    [Server]
    public void ResetBall()
    {
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
        standby = true;
    }

    [Command]
    public void CmdLaunchBall()
    {
        standby = false;

        float RNG = UnityEngine.Random.Range(0f, 1f);
        float horizontalDirection;

        if (RNG > 0.5f)
        {
            horizontalDirection = 1f;
        }
        else
        {
            horizontalDirection = -1f;
        }

        float verticalDirection = UnityEngine.Random.Range(-0.5f, 0.5f);
        rb.AddRelativeForce(new Vector2(horizontalDirection, 0f + verticalDirection) * force);
    }

    [Server]
    private void speedUpBall()
    {
        rb.AddForce(rb.velocity.normalized * 1.3f);
        Debug.Log("SpedUp");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("playerBar"))
        {
            speedUpBall();
        }
    }

}
