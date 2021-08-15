using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Player : MonoBehaviour
{
    //const
    const string FLOWER_MESSAGE_CONTROLLER_TAG = "FlowerMsgController";
    const string GAME_MANAGER_TAG = "GameManager";

    //SerializeField
    [SerializeField] float speed = 3f, jumpPower = 30f;
    
    //protected
    protected FlowerMsgController flowerMsgController;
    protected GameManager gameManager;
    protected PhotonView PV;
    protected bool ableToMove; // active or nonactive

    ////private
    private int jumpCount = 2;
    private Vector3 movement;
    private int playerID;

    public virtual void Awake() {
        //flowerMsgController 초기화
        var MessageControllerObj = GameObject.FindWithTag(FLOWER_MESSAGE_CONTROLLER_TAG); 
        flowerMsgController = MessageControllerObj.GetComponent<FlowerMsgController>();

        //gameManager 초기화
        var GameManagerObj = GameObject.FindWithTag(GAME_MANAGER_TAG);
        gameManager = GameManagerObj.GetComponent<GameManager>();

        //Photon View 초기화
        PV = GetComponent<PhotonView>();
        ableToMove = true;

        //movement 초기화
        movement = Vector3.zero;

        if (PV.IsMine)
        {
            playerID = gameManager.registerPlayer();
        }
    }

    public virtual void Start()
    {
        //카메라 분리
        if (!PV.IsMine) // 플레이어와 애너미에 카메라 모두 붙어있는 상태에서(카메라 4개 생성)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject); // 내꺼가 아니면 카메라 파괴 -> 카메라 2개 남음
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // 내꺼만 움직이도록
        if (!PV.IsMine) return;
        Move();
        Jump();
    }

    public virtual void Move()
    {
        float horizonal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movement = new Vector3(horizonal, 0f, vertical);
    }

    public virtual void Jump()
    {
        if (Input.GetButtonDown("Jump") == true && jumpCount > 0)
        {
            movement = new Vector3(movement.x, jumpPower, movement.z);
            jumpCount--;
        }
    }

    public virtual void FixedUpdate()
    {
        if (!PV.IsMine) return; // 내가 아니면 이동하지 않기
        if (!ableToMove) return;// nonactive 상태면 이동하지 않기

        //LERP 해줘야함
        transform.Translate(movement * Time.fixedDeltaTime * speed); // 나라면 이동하기
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall") jumpCount = 2;
    }


    public PhotonView getPV()
    {
        return PV;
    }

    public int getPlayerID()
    {
        return playerID;
    }

    public bool isPlayerID(int id)
    {
        print("playerid:"+playerID);
        print("this id??:"+id);
        return playerID == id;
    }
}
