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
    const string RUNNER_TAG = "Runner";

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject cameraPrefab;

    //[SerializeField] Button startbtn;
    PhotonView PV;
    GameManager Gamemanager; 
    GameObject[] RunnerObj;
    Vector3 runnerScale, runnerPosition;

    int runnercnt;
    private string playerid;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        //gameManager 초기화
        var GameManagerObj = GameObject.FindWithTag(GAME_MANAGER_TAG);
        Gamemanager = GameManagerObj.GetComponent<GameManager>();
        RunnerObj = GameObject.FindGameObjectsWithTag(RUNNER_TAG);

    }
    private void Start()
    {
            //startbtn.onClick.AddListener(btnOnClick);

            // 일단 방장이 애너미
            // 랜덤으로 애너미 정할 때, 문제점) 클론 때문에 플레이어가 2명이어도 4명으로 인식하는 문제 - 플레이어의 정보를 가져와서 상태를 바꾸는 것이 어려움(4명중 누구의 상태를 바꿔야하는지)
            if (PhotonNetwork.IsMasterClient)
            {

                Vector3 enemyPosition = new Vector3(1, 1.5f, 36); // 방장 술랳
                GameObject gameO =  PhotonNetwork.Instantiate(enemyPrefab.name, enemyPosition, Quaternion.identity);
              
                Player player = gameO.GetComponent<Player>();

                GameObject camera = Instantiate(cameraPrefab, new Vector3(0, 10, 0), cameraPrefab.transform.rotation);
                CameraManager cm = camera.GetComponent<CameraManager>();
                cm.target = gameO.transform;

                player.cam = camera;
            
                //player.setplayerID(PhotonNetwork.NickName);

                print("enemy instatiate");
                
            }
            else
            {
                runnercnt = Gamemanager.getRunnercnt();
                print("runnercnt");
                print(runnercnt); // 계속 0 출력됨..

                if (runnercnt > 0) // 이후의 러너들 // 안들어가짐..
                {
                    runnerScale = RunnerObj[0].transform.lossyScale;
                    print("runner scale");
                    print(runnerScale);
                    runnerPosition = new Vector3(1 + runnerScale.x * 3 * runnercnt, 1.5f, -48); // 러너 위치선정

                }
                else if(runnercnt==0) // 첫번째 러너
                {
                    runnerPosition = new Vector3(1, 1.5f, -48); // 처음 러너 위치 // 계속 이 위치로 생성됨..
                }

                GameObject gameO =PhotonNetwork.Instantiate(playerPrefab.name, runnerPosition, Quaternion.identity);
                
                Player player = gameO.GetComponent<Player>();

                GameObject camera = Instantiate(cameraPrefab, new Vector3(0, 10, 0), cameraPrefab.transform.rotation);
                CameraManager cm = camera.GetComponent<CameraManager>();
                cm.target = gameO.transform;
             
                player.cam = camera;


                // player.setplayerID(PhotonNetwork.NickName);

                print("runner instatiate");

                // Gamemanager.printallplayers();
            }

            Gamemanager.gameStartState();
        
    }
    public void setplayerID(string id)
    {
        playerid = id;
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

