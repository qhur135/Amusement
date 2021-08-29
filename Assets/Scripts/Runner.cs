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

    PlayerInfo playerinfo;

    public override void Awake()
    {
        base.Awake();
        godmode = true;
        passStartLine = false;
        isCaught = false;
        print("Runner Awake");


        playerinfo = GetComponent<PlayerInfo>();
    }
    //public override void Start()
    //{
    //    base.Start();
    //}

    // Update is called once per frame
    public bool IsRunnerCaught()
    {
        return isCaught;
    }
    public override void Update()
    {
        //if (!PV.IsMine) return;
        base.Update();

        if (passStartLine)
        {
            godmode = false; // 선 지나면 무적 상태 풀림
        }
        else // 선안으로 다시 들어가면 무적모드 된다
        {
            godmode = true;
        }


        if (flowerMsgController.isFlowerEnd())
        {
            if (!isCaught && !godmode && lastPosition != transform.position)
            {
                isCaught = true;
                print("딱걸렸어!");
                gameManager.catchPlayer(this);
            }
        }
        else // 멘트 다 외치기 전에 계속해서 최근 위치 업데이트
        {
            lastPosition = transform.position;
            //print("position update");   
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

        Debug.Log("Caughted");
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
            print("pass start line");
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