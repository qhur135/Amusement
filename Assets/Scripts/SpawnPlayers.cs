using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SpawnPlayers : MonoBehaviour
{

    const string GAME_MANAGER_TAG = "GameManager";

    public GameObject playerPrefab;
    public GameObject bouncePrefab;
    public GameObject enemyPrefab;

    //[SerializeField] Button startbtn;
    PhotonView PV;
    GameManager Gamemanager;



    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        //gameManager 초기화
        var GameManagerObj = GameObject.FindWithTag(GAME_MANAGER_TAG);
        Gamemanager = GameManagerObj.GetComponent<GameManager>();

    }
    private void Start()
    {
            //startbtn.onClick.AddListener(btnOnClick);

            // 일단 방장이 애너미
            // 랜덤으로 애너미 정할 때, 문제점) 클론 때문에 플레이어가 2명이어도 4명으로 인식하는 문제 - 플레이어의 정보를 가져와서 상태를 바꾸는 것이 어려움(4명중 누구의 상태를 바꿔야하는지)
            if (PhotonNetwork.IsMasterClient)
            {

                Vector3 enemyPosition = new Vector3(1, 1.5f, 30); // 처음 시작할 때는 모두 러너
                PhotonNetwork.Instantiate(enemyPrefab.name, enemyPosition, Quaternion.identity);

                print("enemy instatiate");
                
            }
            else
            {

                Vector3 runnerPosition = new Vector3(1, 1.5f, -29); // 처음 시작할 때는 모두 러너
                PhotonNetwork.Instantiate(playerPrefab.name, runnerPosition, Quaternion.identity);

                print("runner instatiate");
                //Vector3 runnerPosition = new Vector3(2, 1.5f, -36); // 처음 시작할 때는 모두 러너
                //PhotonNetwork.Instantiate(playerPrefab.name, runnerPosition, Quaternion.identity);

                //print("client runner instatiate");

            }
        
    }
    //public void btnOnClick()
    //{
    //    if (PhotonNetwork.IsMasterClient) // 방장이면 랜덤으로 애너미 고르도록
    //    {
    //        print("master btn click");
    //        PV.RPC("btncolorchange_RPC", RpcTarget.All);

    //        PhotonView enemy = Gamemanager.getEnemy();
    //        PV.RPC("makeenemy_RPC", RpcTarget.All, enemy);
    //    }
    //}
    //[PunRPC]
    //void btncolorchange_RPC()
    //{
    //    startbtn.GetComponent<Button>().image.color = Color.green;
    //}
    //[PunRPC]
    //void makeenemy_RPC(PhotonView enemy)
    //{
    //    //GameObject enemyobject = enemy.GetComponent<GameObject>();
    //    // 애너미로 바꾸기
    //    enemy.transform.Translate(1, 1.5f, 30); // 위치 변경
    //    enemy.GetComponent<Runner>().enabled = false; // 스크립트 변경
    //    enemy.GetComponent<Enemy>().enabled = true;

    //    enemy.GetComponent<Renderer>().material.color = Color.red; // 색 변경
    //    enemy.tag = "Enemy"; // 태그 변경
    //    print("enemy generate");
    //}
}

