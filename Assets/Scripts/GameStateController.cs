using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameStateController : GameManager
{
    const string CHANGE = "StateChange_RPC";

    public Text gameState;

    public void startGame()
    {
        PV.RPC(CHANGE, RpcTarget.All, "GAME START");
 
    }

    
    public void runnerTouch() // runner 닉네임 넘겨줘도 괜찮을 것 같움 !
    {
        PV.RPC(CHANGE, RpcTarget.All, "RUNNER TOUCH ENEMY !");
    }

    public void enemyCatch() //runnner 닉네임 넘겨줘도 괜찮을 것 같움 !!
    {
        PV.RPC(CHANGE, RpcTarget.All, "ENEMY CATCH RUNNER !");
    }

    public void runnerWin()
    {
        PV.RPC(CHANGE, RpcTarget.All, "RUNNER WIN");
    }

    public void cleanText()
    {
        StartCoroutine(timeDelay(2));
    }

    [PunRPC]
    void StateChange_RPC(string state)
    {
        gameState.text = state;
    }

    IEnumerator timeDelay(int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        PV.RPC(CHANGE, RpcTarget.All, "");
    }
}
