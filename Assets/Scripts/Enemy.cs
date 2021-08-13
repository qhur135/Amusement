using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Enemy : Player
{
    const string RUUNER_TAG = "Runner";

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
        base.OnCollisionEnter(collision);

        
        if (collision.gameObject.tag.Equals(RUUNER_TAG))
        {
            ableToMove = true;
        }
    }

    public override void FixedUpdate()
    {
        if (!PV.IsMine) return;
        
        base.FixedUpdate();
    }

    //IEnumerator timeDelay(int delayTime)
    //{
    //    yield return new WaitForSeconds(delayTime);
    //}

    //private void caughtTouched()
    //{
    //    gameObject.GetComponent<PlayerCtrl>().enabled = true;
    //}

    //private void enemyWin()
    //{
    //    gameObject.tag = "NonActive";
    //    gameObject.GetComponent<Enemy>().enabled = false;
    //    gameObject.GetComponent<Player>().enabled = true;
    //    gameObject.transform.position = new Vector3(1, 1.5f, -30);
    //}
}
