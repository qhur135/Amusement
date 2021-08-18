using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Player 
{

    const string START_LINE_TAG = "StartLine";
    const string ENEMY_TAG = "Enemy";
    const string RUNNER_TAG = "Runner";


    Vector3 lastPosition;
    bool godmode; // 무적 상태 - 움직여도 안걸림 
    bool passStartLine; 
    bool isCaught;
  


    public override void Awake()
    {
        base.Awake();
        godmode =true; 
        passStartLine = false; 
        isCaught = false;
        print("Runner Awake");
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

    }

    // Update is called once per frame
    public override void Update()
    {
        if (!PV.IsMine) return;
        base.Update();

       
        if (flowerMsgController.isFlowerEnd()) 
        {
            
            if (godmode)
            {
                lastPosition = transform.position; 
                if (passStartLine)
                {
                    godmode = false; 
                }
            }

           
            if (!godmode && lastPosition != transform.position) 
            {
                isCaught = true;
                gameManager.catchPlayer(this);
            }
        }
    }
    public void tagChange()
    {
        PV.RPC("TagChange", RpcTarget.All);
    }
    IEnumerator timeDelay(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

    [PunRPC]
    void catchPlayer_RPC(Vector3 position)
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 180, 0);
        ableToMove = false;
        godmode = true;
        isCaught = true;
    }

    public override void FixedUpdate()
    {
        if (!PV.IsMine) return;

        base.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(START_LINE_TAG))
        {
            passStartLine = !passStartLine;
        }
    }

    [PunRPC]
    void TagChange()
    {
        gameObject.tag = ENEMY_TAG;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
}
