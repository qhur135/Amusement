using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Player 
{
    // Runner - 술래가 아닌 사람

    const string START_LINE_TAG = "StartLine";

    Vector3 lastPosition;
    bool cannotMove; // 움직여도 되는 상태인지
    bool passStartLine; // 스타트라인을 지나갔는지 확인
    bool isCaught;

    ////int caught_cnt = 0; // 잡힌 플레이어 수
    //int pos_temp; // 연산을 위해 잠시 저장해 둘 변수
    //Vector3 caught_position; // 플레이어 붙잡힐 위치

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

 
    public override void Awake()
    {
        base.Awake();
        cannotMove = false; // 선 안에 있으면 움직여도 되는 상태
        passStartLine = false; 
        isCaught = false; // 잡히지 않은 상태
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

        // 무궁화 꽃이 피었습니다가 끝났다면
        if (flowerMsgController.isFlowerEnd()) 
        {
            // 움직여도 되는 상태라면(선안에 있다면)
            if (!cannotMove)
            {
                lastPosition = transform.position; // 위치 저장
                if (passStartLine)
                {
                    cannotMove = true; // 선을 지났다면 움직일 수 없는 상태로
                }
            }

            // 움직이면 안되는 상태일 때
            // 최근 위치와 현재 위치가 다르다면
            if (cannotMove && lastPosition != transform.position) 
            {
                isCaught = true;
                gameManager.catchPlayer(this);
            }
        }

        //if (spawn.gameTxt.text == "Enemy Win !")
        //{
        //    StartCoroutine(gameEnd());
        //}
        //if (spawn.gameTxt.text == "TOUCH" && spawn.caught_cnt > 0)
        //{
        //    caughtTouched();

        //}
    }

    [PunRPC]
    void catchPlayer_RPC(Vector3 position)
    {
        transform.position = position;
        ableToMove = false;
        cannotMove = false;
        isCaught = true;
    }

    public override void FixedUpdate()
    {
        if (!PV.IsMine) return;

        base.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 스타트 라인을 지나갔다면
        if (other.gameObject.tag.Equals(START_LINE_TAG))
        {
            passStartLine = !passStartLine;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.tag == "StartLine")
    //    {
    //        gameObject.tag = "Active";
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Caught")
    //    {
    //        Debug.Log("player touch caught player");
    //        spawn.gameTxt.text = "TOUCH";
    //        spawn.chageGameTxt();
    //        StartCoroutine(timeDelay(3));
    //        spawn.gameTxt.text = "";
    //        spawn.chageGameTxt();
    //    }
    //    if (collision.gameObject.tag == "Enemy") // 플레이어가 부딪힌 대상이 에너미 일때
    //    {
    //        if (collision.gameObject.GetComponent<PlayerCtrl>().enabled == false)
    //        {
    //            Debug.Log("player touch enemy");
    //            spawn.gameTxt.text = "TOUCH";
    //            spawn.chageGameTxt();
    //            collision.gameObject.GetComponent<PlayerCtrl>().enabled = true;
    //            StartCoroutine(timeDelay(3));
    //            spawn.gameTxt.text = "";
    //            spawn.chageGameTxt();
    //        }
    //        else
    //        {
    //            Debug.Log("Enemy catch player");
    //            gameObject.tag = "Enemy";

    //            //spawn.gameTxt.text = "ENEMY WIN !";
    //            //spawn.chageGameTxt();
    //            //StartCoroutine(timeDelay(3));
    //            //spawn.gameTxt.text = "START !";
    //            //spawn.chageGameTxt();
    //            //StopCoroutine(timeDelay(3));
    //            //spawn.gameTxt.text = "";
    //            //spawn.chageGameTxt();
    //        }

    //    }
    //}

    //IEnumerator gameEnd()
    //{
    //    yield return new WaitForSeconds(1);

    //    if(gameObject.tag == "Enemy")
    //    {
    //        //chage position to Enemy's default position;
    //        gameObject.transform.position = new Vector3(1, 2, 28);
    //    }
    //    else
    //    {
    //        //change position to startLine
    //        gameObject.tag = "NonActive";
    //        gameObject.transform.position = new Vector3(1, 1.5f, -36);
    //    }
    //}

    //IEnumerator timeDelay(int delayTime)
    //{
    //    yield return new WaitForSeconds(delayTime);
    //}


    //private void caughtTouched()
    //{
    //    gameObject.tag = "Acitve";

    //    gameObject.GetComponent<PlayerCtrl>().enabled = true; // 포지션 정보 넣어서 뒤에 있는 아이들만 풀어줘야 ㅎ
    //}
}
