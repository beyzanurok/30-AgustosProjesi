using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//ağ yoneticisi
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public int maxPlayers = 10;

    // instance
    public static NetworkManager instance;

    void Awake ()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        // ana sunucu bağlantısı
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster ()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom (string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayers;

        PhotonNetwork.CreateRoom(roomName, options);
    }
    
    
    public void JoinRoom (string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    
    [PunRPC]
    public void ChangeScene (string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    // bağlantı kopunca menuye yonlendirme
    public override void OnDisconnected (DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("Menu");
    }

    //odadan oyuncu çıkıncı bilgilendirme
    public override void OnPlayerLeftRoom (Player otherPlayer)
    {
        GameManager.instance.alivePlayers--;
        GameUI.instance.UpdatePlayerInfoText();

        if(PhotonNetwork.IsMasterClient)
        {
            GameManager.instance.CheckWinCondition();
        }
    }
}