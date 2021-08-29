using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Player : MonoBehaviour
{
    //const
    const string FLOWER_MESSAGE_CONTROLLER_TAG = "FlowerMsgController";
    const string GAME_MANAGER_TAG = "GameManager";
    
    public GameObject cam;

    //SerializeField
    [SerializeField] float speed = 11.0f, jumpPower = 2.0f;
    
    //protected
    protected FlowerMsgController flowerMsgController;
    protected GameManager gameManager;
    protected PhotonView PV;
    protected bool ableToMove; // active or nonactive

    //private
    private string playerID;
    private Rigidbody rb;
    private bool speedup;

    private float airVelocity = 7f;
    private float gravity = 40.0f;
    private float maxVelocityChange = 10.0f;
    private float maxFallSpeed = 20.0f;
    private float rotateSpeed = 25f; //Speed the player rotate
    private Vector3 moveDir;

    private float distToGround;

    private bool canMove = true; //If player is not hitted
    private bool isStuned = false;
    private bool wasStuned = false; //If player was stunned before get stunned another time
    private float pushForce;
    private Vector3 pushDir;

    //public Vector3 checkPoint;
    private bool slide = false;

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

        // rb 초기화
        rb = GetComponent<Rigidbody>();


        // 스피트상태 초기화
        speedup = false;

        rb.useGravity = false; // ??

        Cursor.lockState = CursorLockMode.Locked;

        //if (PV.IsMine)
        //{
        //    playerID = gameManager.registerPlayer();
        //}
    }

    private void Start()
    {
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    protected void delayTime(int delayTime)
    {
        StartCoroutine(timeDelay(delayTime));
    }
    

    // Update is called once per frame
    public virtual void Update()
    {
        // 내꺼만 움직이도록
        if (!PV.IsMine) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 v2 = v * cam.transform.forward; //Vertical axis to which I want to move with respect to the camera
        Vector3 h2 = h * cam.transform.right; //Horizontal axis to which I want to move with respect to the camera
        moveDir = (v2 + h2).normalized; //Global position to which I want to move in magnitude 1

        if (Input.GetKeyDown(KeyCode.X))
        {
            speedup = true;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround + 0.1f))
        {
            if (hit.transform.tag == "Slide")
            {
                slide = true;
            }
            else
            {
                slide = false;
            }
        }
    }

    public virtual void FixedUpdate()
    {
        if (!PV.IsMine) return; // 내가 아니면 이동하지 않기
        if (!ableToMove) return;// nonactive 상태면 이동하지 않기

        if (canMove)
        {
            if (moveDir.x != 0 || moveDir.z != 0)
            {
                Vector3 targetDir = moveDir; //Direction of the character

                targetDir.y = 0;
                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;
                Quaternion tr = Quaternion.LookRotation(targetDir); //Rotation of the character to where it moves
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * rotateSpeed); //Rotate the character little by little
                transform.rotation = targetRotation;
            }

            if (IsGrounded())
            {
                // Calculate how fast we should be moving
                Vector3 targetVelocity = moveDir;

                if (speedup)
                {
                    targetVelocity *= 20.0f;
                }
                else
                {
                    targetVelocity *= speed;
                }
                
                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                if (targetVelocity.magnitude < velocity.magnitude) //If I'm slowing down the character
                {
                    targetVelocity = velocity;
                    rb.velocity /= 1.1f;
                }
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;
                if (!slide)
                {
                    if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
                        rb.AddForce(velocityChange, ForceMode.VelocityChange);
                }
                else if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
                {
                    rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
                    //Debug.Log(rb.velocity.magnitude);
                }

                // Jump
                if (IsGrounded() && Input.GetButton("Jump"))
                {
                    rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
                }
            }
            else
            {
                if (!slide)
                {
                    Vector3 targetVelocity = new Vector3(moveDir.x * airVelocity, rb.velocity.y, moveDir.z * airVelocity);
                    Vector3 velocity = rb.velocity;
                    Vector3 velocityChange = (targetVelocity - velocity);
                    velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                    rb.AddForce(velocityChange, ForceMode.VelocityChange);
                    if (velocity.y < -maxFallSpeed)
                        rb.velocity = new Vector3(velocity.x, -maxFallSpeed, velocity.z);
                }
                else if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
                {
                    rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
                }
            }
        }
        else
        {
            rb.velocity = pushDir * pushForce;
        }
        // We apply gravity manually for more tuning control
        rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpPower * gravity);
    }

    public void HitPlayer(Vector3 velocityF, float time)
    {
        rb.velocity = velocityF;

        pushForce = velocityF.magnitude;
        pushDir = Vector3.Normalize(velocityF);
        StartCoroutine(Decrease(velocityF.magnitude, time));
    }

    public void LoadCheckPoint()
    {
        //transform.position = checkPoint;
    }

    private IEnumerator Decrease(float value, float duration)
    {
        if (isStuned)
            wasStuned = true;
        isStuned = true;
        canMove = false;

        float delta = 0;
        delta = value / duration;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;
            if (!slide) //Reduce the force if the ground isnt slide
            {
                pushForce = pushForce - Time.deltaTime * delta;
                pushForce = pushForce < 0 ? 0 : pushForce;
            }
            rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0)); //Add gravity
        }

        if (wasStuned)
        {
            wasStuned = false;
        }
        else
        {
            isStuned = false;
            canMove = true;
        }
    }

    IEnumerator timeDelay(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

    public PhotonView getPV()
    {
        return PV;
    }
    //public void setplayerID(string id)
    //{
    //    PV.RPC("setplayerID_RPC", RpcTarget.All, id);

    //    gameManager.appendPlayer(playerID);
    //    //print("set playerid");
    //    //gameManager.printallplayers();
    //}
    public string getPlayerID()
    {
        return playerID;
    }

    public bool isPlayerID(string id)
    {
        print("playerid:"+playerID);
        print("this id??:"+id);
        return playerID == id;
    }
    [PunRPC]
    void setplayerID_RPC(string id)
    {
        playerID = id;
        print(playerID);
    }
    
}
