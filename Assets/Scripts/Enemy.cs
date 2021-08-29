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
            PV.RPC(ENEMY_TURN, RpcTarget.All); // 멘트 모두 외치고 러너 바라보도록
        }
    }

    public void OnCollisionEnter(Collision collision) // oncollisionEnter은 계속 호출되서 터치하고 때는 순간 한번 호출되도록
    {

        if (!PV.IsMine) return;

        if (collision.gameObject.tag.Equals(RUNNER_TAG))
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

                base.delayTime(5);

                PV.RPC("colorChangeToRunner", RpcTarget.All); // 러너가 된다
                PV.RPC(ENEMY_TURN, RpcTarget.All); // 러너가 되어서 애너미를 바라봄

                base.delayTime(3);
            }
        }

    }

    public override void FixedUpdate()
    {
        if (!PV.IsMine) return;

        base.FixedUpdate();
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
        float view = transform.rotation.z;
        view = (view + 180) > 180 ? 0 : 180;
        // print(view);
        transform.rotation = Quaternion.Euler(0, view, 0);
    }

}