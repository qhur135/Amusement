using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    const string SPAWN_PLAYER_TAG = "SpawnPlayers";

    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_InputField nameInput;

    private void Start()
    {
        print("hi");


    }
    public void CreateRoom()
    {
        PhotonNetwork.NickName = nameInput.text;
        PhotonNetwork.CreateRoom(createInput.text);
        print("create room");
    }
    public void JoinRoom()
    {
        PhotonNetwork.NickName = nameInput.text;
        PhotonNetwork.JoinRoom(joinInput.text);
        print("join room");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
        
    }
}