using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PongNetworkManager : NetworkManager
{
    public List<PongPlayer> Players { get; } = new List<PongPlayer>();

    [SerializeField] private GameObject PlayerBarPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private GameObject BallPrefab;
    [SerializeField] private GameOverHandeler gameOverHandlerPrefab;

    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    private bool isGameInProgress = false;
    public bool IsGameInProgress { get { return isGameInProgress; } }


    #region Server

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        PongPlayer player = conn.identity.GetComponent<PongPlayer>();
        Players.Add(player);

        player.SetPlayerName($"Player {Players.Count}");

        Color col = new Color(
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f)
            );

        player.SetPlayerColour(col);


        player.SetIsPartyOwner(Players.Count == 1);


    }



    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Map"))
        {
            GameOverHandeler gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(gameOverHandlerInstance.gameObject);


            GameObject ballInstance = Instantiate(BallPrefab);
            NetworkServer.Spawn(ballInstance.gameObject, Players[0].connectionToClient);


            foreach (PongPlayer player in Players)
            {
                GameObject playerInstance = Instantiate(PlayerBarPrefab, GetStartPosition().position, Quaternion.identity);
                NetworkServer.Spawn(playerInstance, player.connectionToClient);

                GameObject GoalInstance = Instantiate
                    (goalPrefab, 
                    -playerInstance.transform.position + 
                    new Vector3(1.2f * (-playerInstance.transform.position.x / Mathf.Abs(playerInstance.transform.position.x)),0,0), 
                    Quaternion.identity);

                NetworkServer.Spawn(GoalInstance, player.connectionToClient);
            }

        }
    }
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (!isGameInProgress) return;

        conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        PongPlayer player = conn.identity.GetComponent<PongPlayer>();
        Players.Remove(player);
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        Players.Clear();

        isGameInProgress = false;
    }

    public void StartGame()
    {
        if (Players.Count < 2) return;

        isGameInProgress = true;

        ServerChangeScene("Map");
    }

    #endregion

    #region Client

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        ClientOnDisconnected?.Invoke();
    }

    public override void OnStopClient()
    {
        Players.Clear();
    }


    #endregion

}
