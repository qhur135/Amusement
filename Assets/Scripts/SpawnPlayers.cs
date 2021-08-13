using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject bouncePrefab;

    //private int currentidx;
    //

    //string[] text = new string[10] { "무", "궁", "화 ", "꽃", "이 ", "피", "었", "습", "니", "다" };

    private void Start()
    {
       // Vector3 bouncePosition = new Vector3(1, 3, -29);
        //PhotonNetwork.Instantiate(bouncePrefab.name, bouncePosition, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient) // enemy - create room
        {
            //currentidx = 0;
            // Vector3 enemyPosition = new Vector3(1, 2, 28);
            Vector3 enemyPosition = new Vector3(1, 2, -30);

            PhotonNetwork.Instantiate(enemyPrefab.name, enemyPosition, Quaternion.identity);
        }
        else
        {
            Vector3 playerPosition = new Vector3(1, 1.5f, -36);
            PhotonNetwork.Instantiate(playerPrefab.name, playerPosition, Quaternion.identity);
        } 
       
    }
    void Update()
    {
        //if (GameObject.FindWithTag("Enemy")) // enemy
        //{
        //    if (Input.GetKeyDown(KeyCode.LeftControl))
        //    {
        //        if (currentidx >= 10)
        //        {
        //            return;
        //        }

                
        //        stateTxt.text += text[currentidx];
        //        
                
        //        currentidx++;
        //    }

        //    if (Input.GetKeyDown(KeyCode.Return))
        //    {
        //        stateTxt.text = "";
        //        view.RPC("Push_Enter", RpcTarget.All,null);
               
        //        currentidx = 0;
        //    }
        //}

    }
       
}

