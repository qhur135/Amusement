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

    public GameObject enemyPrefab;
    public GameObject runnerPrefab;
    public GameObject bouncePrefab;

    [SerializeField] Button startbtn;
    PhotonView PV;
    GameManager Gamemanager;
    Player players;



    private void Awake()
    {
        PV = GetComponent<PhotonView>();


        //gameManager 초기화
        var GameManagerObj = GameObject.FindWithTag(GAME_MANAGER_TAG);
        Gamemanager = GameManagerObj.GetComponent<GameManager>();
    }
    private void Start()
    {
        startbtn.onClick.AddListener(btnOnClick);

        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 runnerPosition = new Vector3(1, 1.5f, -36); // 처음 시작할 때는 모두가 러너
            PhotonNetwork.Instantiate(runnerPrefab.name, runnerPosition, Quaternion.identity);

            print("runner instatiate");
        }
        else
        {
            Vector3 runnerPosition = new Vector3(2, 1.5f, -36); // 처음 시작할 때는 모두가 러너
            PhotonNetwork.Instantiate(runnerPrefab.name, runnerPosition, Quaternion.identity);
        }
    }
    public void btnOnClick()
    {
        if (PhotonNetwork.IsMasterClient) // 방장이면 랜덤으로 애너미 고르도록
        {
            PV.RPC("btncolorchange_RPC", RpcTarget.All);

            PhotonView enemy = Gamemanager.getEnemy();
            GameObject enemyobject = enemy.GetComponent<GameObject>();
            // 애너미로 프리팹 바꾸기

            //// 애너미 
            //Vector3 enemyPosition = new Vector3(1, 1.5f, -30); 
            //PhotonNetwork.Instantiate(enemy.name, enemyPosition, Quaternion.identity);
            //print("generate");
            
        }
    }
    [PunRPC]
    void btncolorchange_RPC()
    {
        startbtn.GetComponent<Button>().image.color = Color.green;
    }
}

