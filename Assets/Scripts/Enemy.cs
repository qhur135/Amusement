using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Enemy : Player
{
    const string RUNNER_TAG = "Runner";
    const string ENEMY_TAG = "Enemy";

    public override void Awake()
    {
        base.Awake();
        if (!PV.IsMine) return;
<<<<<<< Updated upstream
        ableToMove = false;
        Debug.Log("Enemy Awake");
        
=======
        ableToMove = false;

>>>>>>> Stashed changes
    }

    public override void Update()
    {
        if (!PV.IsMine) return;
<<<<<<< Updated upstream

        base.Update();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            flowerMsgController.CountFlowerOnce();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            flowerMsgController.ResetFlower();
        }

=======
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
        
>>>>>>> Stashed changes
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (!PV.IsMine || gameObject.tag != ENEMY_TAG) return;
        base.OnCollisionEnter(collision);
<<<<<<< Updated upstream
        
=======
>>>>>>> Stashed changes
    }

    public void OnCollisionExit(Collision collision) // oncollisionEnter은 계속 호출되서 터치하고 때는 순간 한번 호출되도록
    {
        if (!PV.IsMine) return;

        if (collision.gameObject.tag.Equals(RUNNER_TAG) && !ableToMove)
        {
            ableToMove = true;
            Debug.Log("Runner touch enemy");
        }
        else if (collision.gameObject.tag.Equals(RUNNER_TAG) && ableToMove)
        {
            Debug.Log("Enemy catch runner");
<<<<<<< Updated upstream

            //gameObject.tag = RUNNER_TAG;
            //Debug.Log(gameObject.tag);

            Runner newE = collision.gameObject.GetComponent<Runner>();
            newE.tagChange(); 

            StartCoroutine(timeDelay(2));
            //gameManager.restartGame();

            PV.RPC("colorChangeToRunner", RpcTarget.All);
            
=======
            Runner newE = collision.gameObject.GetComponent<Runner>();
            newE.tagChange();

            StartCoroutine(timeDelay(2));

            PV.RPC("colorChangeToRunner", RpcTarget.All);

>>>>>>> Stashed changes
        }
    }

    

    public override void FixedUpdate()
    {
        if (!PV.IsMine) return;

        base.FixedUpdate();
<<<<<<< Updated upstream
    }

=======
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

    IEnumerator timeDelay(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

>>>>>>> Stashed changes
    [PunRPC]
    void colorChangeToRunner()
    {
        gameObject.tag = RUNNER_TAG;
        Debug.Log(gameObject.tag);

        //StartCoroutine(timeDelay(2));
        gameManager.restartGame();

        gameObject.GetComponent<Renderer>().material.color = Color.blue;

    }

    

    IEnumerator timeDelay(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }
    
}
