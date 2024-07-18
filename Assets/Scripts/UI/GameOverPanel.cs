using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject gameOverDisplayParent;
    [SerializeField] private TMP_Text winnerNameText;

    private void OnEnable()
    {
        GameOverHandeler.ClientOnGameOver += ClientHandleGameOver;
    }

    private void OnDisable()
    {
        GameOverHandeler.ClientOnGameOver -= ClientHandleGameOver;
    }

    private void ClientHandleGameOver(int winner)
    {

        if (winner == 0)
        {
            winnerNameText.text = $"Player 1 Wins";
        }
        else
        {
            winnerNameText.text = $"Player 2 Wins";
        }


        gameOverDisplayParent.SetActive(true);
    }

    public void LeaveGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }

        SceneManager.LoadScene(0);
    }
}
