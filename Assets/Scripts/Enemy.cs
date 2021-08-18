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
        if (!PV.IsMine) return;
        ableToMove = false;
        print("Enemy Awake");

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
            PV.RPC(ENEMY_TURN, RpcTarget.All); // 플레이어 스크립트에 있음
        }
    }

    public override void OnCollisionEnter(Collision collision) // oncollisionEnter은 계속 호출되서 터치하고 때는 순간 한번 호출되도록
    {
        
        base.OnCollisionEnter(collision);

        if (!PV.IsMine) return;

        if(collision.gameObject.tag.Equals(RUNNER_TAG))
        {
            if (!ableToMove)
            {
                ableToMove = true; // 애너미 움직이게 됨
                print("Runner touch enemy");
            }
            else
            {
                print("Enemy catch runner");
                Runner newE = collision.gameObject.GetComponent<Runner>();
                newE.tagChange();

                StartCoroutine(timeDelay(2));

                PV.RPC("colorChangeToRunner", RpcTarget.All);
            }
        }

    

    }

    public override void FixedUpdate()
    {
        if (!PV.IsMine) return;

        base.FixedUpdate();
    }

    IEnumerator timeDelay(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

    [PunRPC]
    void colorChangeToRunner()
    {
        gameObject.tag = RUNNER_TAG;
        Debug.Log(gameObject.tag);

        //StartCoroutine(timeDelay(2));
        gameManager.restartGame();

        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }
    [PunRPC]
    void enemyTurn()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
   

    //private void enemyWin()
    //{
    //    gameObject.tag = "NonActive";
    //    gameObject.GetComponent<Enemy>().enabled = false;
    //    gameObject.GetComponent<Player>().enabled = true;
    //    gameObject.transform.position = new Vector3(1, 1.5f, -30);
    //}
}
