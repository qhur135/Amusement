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
    [SerializeField] float speed = 5f, jumpPower = 5f;
    
    //protected
    protected FlowerMsgController flowerMsgController;
    protected GameManager gameManager;
    protected PhotonView PV;
    protected bool ableToMove; // active or nonactive

    //private
    private int jumpCount = 2;
    private Vector3 movement;
    private bool isJumping;
    private int playerID;
    private Rigidbody rb;
    private float horizonal;
    private float vertical;
    private bool speedup;

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

        // rb 초기화
        rb = GetComponent<Rigidbody>();

        // 점프상태 초기화
        isJumping = false;

        // 스피트상태 초기화
        speedup = false;

        //if (PV.IsMine)
        //{
        //    playerID = gameManager.registerPlayer();
        //}
    }

    //public virtual void Start()
    //{
    //    //카메라 분리
    //    if (!PV.IsMine) // 플레이어와 애너미에 카메라 모두 붙어있는 상태에서(카메라 4개 생성)
    //    {
    //        Destroy(GetComponentInChildren<Camera>().gameObject); // 내꺼가 아니면 카메라 파괴 -> 카메라 2개 남음
    //    }
    //}

    // Update is called once per frame
    public virtual void Update()
    {
        // 내꺼만 움직이도록
        if (!PV.IsMine) return;

        // 키 입력 받기
        horizonal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Jump") == true && jumpCount > 0)
           isJumping = true;

        if (Input.GetKeyDown(KeyCode.X))
        {
            speedup = true; // 빨라지도록
            //print("true");
        }
        else if(Input.GetKeyUp(KeyCode.X))
        {
            speedup = false;
            //print("false");
        }
    }

    public virtual void Move()
    {
        if (speedup)
        {
            speed = 10f; // 빨라지도록
            print("speedup!");
        }
        else
        {
            speed = 5f;
            print("origin speed");
        }
        movement = new Vector3(horizonal, 0f, vertical);
        movement = movement.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement); // 현재위치 + 움직인 위치
    }

    public virtual void Jump()
    {
        if (!isJumping)
            return;

        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse); // 점프 내려오는 속도가 느린 문제..? 조금 더 빨리 내려오도록..?
        isJumping = false;
        jumpCount--;
    }

    public virtual void FixedUpdate()
    {
        if (!PV.IsMine) return; // 내가 아니면 이동하지 않기
        if (!ableToMove) return;// nonactive 상태면 이동하지 않기

        // 물리적 처리
        Move();
        Jump();

        //transform.Translate(movement * Time.fixedDeltaTime * speed); // 나라면 이동하기

        // lerp - 부드럽게 이동, 점프
        //transform.position = Vector3.Lerp(transform.position, movement, Time.fixedDeltaTime);
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
