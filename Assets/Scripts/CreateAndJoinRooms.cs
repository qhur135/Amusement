using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_InputField nameInput;

    public string PlayerID { get; set; }

    private void Awake()
    {
        PlayerID = "";
        DontDestroyOnLoad(this.gameObject);
    }
    public void CreateRoom()
    {
        PlayerID = nameInput.text;
        PhotonNetwork.CreateRoom(createInput.text);
        print("create room");
    }
    public void JoinRoom()
    {
        PlayerID = nameInput.text;
        PhotonNetwork.JoinRoom(joinInput.text);
        print("join room");
    }
    public override void OnJoinedRoom()
    {
        print("joinroom");
        PhotonNetwork.LoadLevel("Game");
        
    }
}