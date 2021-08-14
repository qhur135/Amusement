using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Player 
{

    const string START_LINE_TAG = "StartLine";

    Vector3 lastPosition;
    bool cannotMove; 
    bool passStartLine; 
    bool isCaught;

    ////int caught_cnt = 0; 
    //int pos_temp; 
    //Vector3 caught_position; 

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

 
    public override void Awake()
    {
        base.Awake();
        cannotMove = false; 
        passStartLine = false; 
        isCaught = false; 
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
            
            if (!cannotMove)
            {
                lastPosition = transform.position; 
                if (passStartLine)
                {
                    cannotMove = true; 
                }
            }

           
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
    //    if (collision.gameObject.tag == "Enemy") // ?????????? ?????? ?????? ?????? ????
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

    //    gameObject.GetComponent<PlayerCtrl>().enabled = true; // ?????? ???? ?????? ???? ???? ???????? ???????? ??
    //}
}
