using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text playeridtext; 

    const string UPDATE_Player_ID = "UpdatePlayerID_RPC";
    PhotonView pv;

    public string PlayerID { get; set; }
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    [PunRPC]
    public void UpdatePlayerID_RPC(string id)
    {
        PlayerID = id;
        playeridtext.text = PlayerID;
    }
    public void SetPlayerID(string id)
    {
        pv.RPC(UPDATE_Player_ID, RpcTarget.All, id);
    }
    public string getplayerid()
    {
        return PlayerID;
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (!pv.IsMine) return;
        base.OnPlayerEnteredRoom(newPlayer);
    pv.RPC(UPDATE_Player_ID, RpcTarget.All, PlayerID);
    }
}
