using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Enemy : Player
{
    //const
    const string ENEMY_TURN = "enemyTurn";
    const string FLOWER_MESSAGE_CONTROLLER_TAG = "FlowerMsgController";


    const string RUNNER_TAG = "Runner";
    const string ENEMY_TAG = "Enemy";

    public override void Awake()
    {
        base.Awake();
        ableToMove = false;

    }

    public override void Update()
    {
        if (!PV.IsMine) return;
        base.Update(); 
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            flowerMsgController.CountFlowerOnce();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            flowerMsgController.ResetFlower();
        }
        if (flowerMsgController.isFlowerEnd())
        {
            PV.RPC(ENEMY_TURN,RpcTarget.All); // 플레이어 스크립트에 있음
        }
        //if (spawn.gameTxt.text == "TOUCH")
        //{
        //    caughtTouched();
        //}

        //if (spawn.gameTxt.text == "ENEMY WIN !")
        //{
        //    enemyWin();
        //}
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (!PV.IsMine || gameObject.tag != ENEMY_TAG) return;
        base.OnCollisionEnter(collision);

        if (collision.gameObject.tag.Equals(RUNNER_TAG) && !ableToMove)
        {
            ableToMove = true;
            Debug.Log("Runner touch enemy");
        }
        else if (collision.gameObject.tag.Equals(RUNNER_TAG) && ableToMove)
        {
            Debug.Log("Enemy catch a runner");
            
        }
    }

    public override void FixedUpdate()
    {
        if (!PV.IsMine) return;

        base.FixedUpdate();
    }

    void changeEnemy(Collision collision)
    {
        collision.gameObject.tag = ENEMY_TAG;
        collision.gameObject.GetComponent<Runner>().enabled = false;
        collision.gameObject.GetComponent<Enemy>().enabled = true;
        collision.gameObject.transform.position = new Vector3(1, 1.5f, 30);
        collision.gameObject.GetComponent<Renderer>().material.color = Color.red;

        gameObject.tag = RUNNER_TAG;
        gameObject.transform.position = new Vector3(2, 1.5f, -36);
        gameObject.GetComponent<Renderer>().material.color = Color.blue; // 색 변경
        gameObject.GetComponent<Runner>().enabled = true; // 스크립트 변경
        gameObject.GetComponent<Enemy>().enabled = false;
    }
    [PunRPC]
    void enemyTurn()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    //IEnumerator timeDelay(int delayTime)
    //{
    //    yield return new WaitForSeconds(delayTime);
    //}

    //private void enemyWin()
    //{
    //    gameObject.tag = "NonActive";
    //    gameObject.GetComponent<Enemy>().enabled = false;
    //    gameObject.GetComponent<Player>().enabled = true;
    //    gameObject.transform.position = new Vector3(1, 1.5f, -30);
    //}
}
