using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    const string CAUGHT_RUNNER = "catchPlayer_RPC";
    const string APEEND_PLAYER = "appendPlayer_RPC";
    const string ENEMY_TAG = "Enemy";
    const string RUNNER_TAG = "Runner";
    const string GAME_STATE_CONTROLLER_TAG = "GameStateController";

    const float GAP = 0.5f;

    List<int> caughtRunners;
    List<string> playerIDs;

    Player Players;
    protected PhotonView PV;
    GameObject[] RunnerObj;
    GameStateController gameStateController;
    int caughtcnt;

    Vector3 basePosition, runnerScale;

    private void Awake()
    {
        caughtcnt = 0;

        PV = GetComponent<PhotonView>();
        caughtRunners = new List<int>();
        playerIDs = new List<string>();

        var gameStateObj = GameObject.FindGameObjectWithTag(GAME_STATE_CONTROLLER_TAG);
        gameStateController = gameStateObj.GetComponent<GameStateController>();
    }
    //public void appendPlayer(string id)
    //{
    //    PV.RPC(APEEND_PLAYER, RpcTarget.All, id);
    //}
    //public void printallplayers()
    //{
    //    for(int i = 0; i < playerIDs.Count; i++)
    //    {
    //        print(playerIDs.Count);
    //        print(playerIDs[i]);
    //    }
    //}

    public void gameStartState()
    {
        gameStateController.startGame();
        gameStateController.cleanText();
    }

    public void runnerTouchState()
    {
        gameStateController.runnerTouch();
        gameStateController.cleanText();
    }

    public void enemyCatchState()
    {
        gameStateController.enemyCatch();
        gameStateController.cleanText();
    }
    public void catchPlayer(Runner runner)
    {
        caughtcnt = 0;
        if(basePosition == Vector3.zero) 
        {
            basePosition = GameObject.FindWithTag(ENEMY_TAG).transform.position;
            runnerScale = runner.transform.lossyScale;
        }

        PhotonView PV = runner.getPV();

        RunnerObj = GameObject.FindGameObjectsWithTag(RUNNER_TAG);
        
        for (int i = 0; i < RunnerObj.Length; i++)
        {
            if (RunnerObj[i].GetComponent<Runner>().IsRunnerCaught()) // 잡힌 러너라면 카운트
            {
                caughtcnt = caughtcnt + 1;
                print("caught cnt up");
            }
        }
        //this.PV.RPC(CAUGHT_RUNNER, RpcTarget.All, runner.getPlayerID()); // 잡힌 애들 저장하기
        //print(caughtcnt);
        //print(basePosition.z);
        //Vector3 nextPosition = new Vector3(basePosition.x + runnerScale.x * caughtRunners.Count, basePosition.y, basePosition.z);
        Vector3 nextPosition = new Vector3(basePosition.x + runnerScale.x*2*caughtcnt , basePosition.y, basePosition.z);
        

        //print("catch player");
        PV.RPC(CAUGHT_RUNNER, RpcTarget.All, nextPosition); // 술래 옆에 붙잡아 놓기 , 러너 스크립트에 있
    }
    public int getRunnercnt()
    {
        RunnerObj = GameObject.FindGameObjectsWithTag(RUNNER_TAG);
        return RunnerObj.Length;
    }
    public void restartGame()
    {
        run();
        enemy();

        gameStartState();
    }

    void run()
    {
        PV.RPC("runnerSetting", RpcTarget.All);

    }

    void enemy()
    {
        PV.RPC("enemySetting", RpcTarget.All);
    }
    [PunRPC]
    void appendPlayer_RPC(string id)
    {
        print("id append rpc");
        playerIDs.Add(id);
    }

    [PunRPC]
    void runnerSetting()
    {
        //GameObject[] players = GameObject.FindGameObjectsWithTag(RUNNER_TAG);

        for (int i = 0; i < getRunnercnt(); i++)
        {
            RunnerObj[i].transform.position = new Vector3(runnerScale.x * i + 2, 1.5f, -48);

            Debug.Log(RunnerObj[i].transform.position);

            if (RunnerObj[i].GetComponent<Enemy>().enabled)
            {
                RunnerObj[i].GetComponent<Runner>().enabled = true;
                RunnerObj[i].GetComponent<Runner>().cam = RunnerObj[i].GetComponent<Enemy>().cam;
                RunnerObj[i].GetComponent<Enemy>().enabled = false;
            }

            Runner runner = RunnerObj[i].GetComponent<Runner>();
            runner.Awake();
            //runner.Start();
        }

    }

    [PunRPC]
    void enemySetting()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag(ENEMY_TAG);

        enemy.transform.position = new Vector3(1, 1.5f, 36);
        Debug.Log(enemy.transform.position);

        enemy.GetComponent<Enemy>().enabled = true;
        enemy.GetComponent<Enemy>().cam = enemy.GetComponent<Runner>().cam;
        enemy.GetComponent<Runner>().enabled = false;

        Enemy newEnemy = enemy.GetComponent<Enemy>();
        newEnemy.Awake();
    }


    //[PunRPC]
    //public void catchPlayer_RPC(int playerID)
    //{
    //    caughtRunners.Add(playerID);
    //}
}
