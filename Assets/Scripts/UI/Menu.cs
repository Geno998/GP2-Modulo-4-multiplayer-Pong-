using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject StartPage;

    public void HostLobby()
    {
        StartPage.SetActive(false);

        NetworkManager.singleton.StartHost();
    }
}
