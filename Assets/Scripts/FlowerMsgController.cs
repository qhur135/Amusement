using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class FlowerMsgController : MonoBehaviour
{
    const string UPDATE_FLOWER_STATE = "CountFlowerOnce_RPC",
            RESET_FLOWER_STATE = "ResetFlower_RPC";
    
    [SerializeField] TMP_Text flowerState;
    
    PhotonView PV;
    int flowerCnt; 
    string[] flowerText;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        flowerText = new string[10] { "무", "궁", "화 ", "꽃", "이 ", "피", "었", "습", "니", "" };
        flowerCnt = 0;
    }

    public bool isFlowerEnd()
    {
        return flowerCnt == flowerText.Length;
    }

    public void CountFlowerOnce()
    {
        if (flowerCnt >= flowerText.Length)
        {
            return;
        }

        PV.RPC(UPDATE_FLOWER_STATE, RpcTarget.All);
    }

    public void ResetFlower()
    {
        PV.RPC(RESET_FLOWER_STATE, RpcTarget.All);
    }

    [PunRPC]
    void CountFlowerOnce_RPC()
    {
        flowerState.text += flowerText[flowerCnt++];
    }

    [PunRPC]
    void ResetFlower_RPC()
    {
        flowerState.text = "";
        flowerCnt = 0;
    }
}
