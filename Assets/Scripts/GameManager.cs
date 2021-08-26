using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    const string CAUGHT_RUNNER = "catchPlayer_RPC";
    const string ENEMY_TAG = "Enemy";
    const string RUNNER_TAG = "Runner";
    const float GAP = 0.5f;

    List<int> caughtRunners;
    List<int> playerIDs;

    Player Players;
    PhotonView PV;

    Vector3 basePosition, runnerScale;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        caughtRunners = new List<int>();
        playerIDs = new List<int>();
    }

    public void catchPlayer(Runner runner)
    {
        if(basePosition == Vector3.zero) 
        {
            basePosition = GameObject.FindWithTag(ENEMY_TAG).transform.position;
            runnerScale = runner.transform.lossyScale;
        }

        PhotonView PV = runner.getPV();
        //this.PV.RPC(CAUGHT_RUNNER, RpcTarget.All, runner.getPlayerID()); // 잡힌 애들 저장하기
        Vector3 nextPosition = new Vector3(basePosition.x + runnerScale.x * caughtRunners.Count, basePosition.y, basePosition.z);
        PV.RPC(CAUGHT_RUNNER, RpcTarget.All, nextPosition); // 술래 옆에 붙잡아 놓기 - 러너 코드에 rpc함수있음
    }
    public void restartGame()
    {
        run();
        enemy();
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
    void runnerSetting()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(RUNNER_TAG);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = new Vector3(runnerScale.x * i + 2, 1.5f, -36);

            Debug.Log(players[i].transform.position);

            if (players[i].GetComponent<Enemy>().enabled)
            {
                players[i].GetComponent<Runner>().enabled = true;
                players[i].GetComponent<Runner>().cam = players[i].GetComponent<Enemy>().cam;
                players[i].GetComponent<Enemy>().enabled = false;
            }

            Runner runner = players[i].GetComponent<Runner>();
            runner.Awake();
            //runner.Start();
        }

    }

    [PunRPC]
    void enemySetting()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag(ENEMY_TAG);

        enemy.transform.position = new Vector3(1, 1.5f, 30);
        Debug.Log(enemy.transform.position);

        enemy.GetComponent<Enemy>().enabled = true;
        enemy.GetComponent<Enemy>().cam = enemy.GetComponent<Runner>().cam;
        enemy.GetComponent<Runner>().enabled = false;

        Enemy newEnemy = enemy.GetComponent<Enemy>();
        newEnemy.Awake();
    }


    [PunRPC]
    public void catchPlayer_RPC(int playerID)
    {
        caughtRunners.Add(playerID);
    }
}
