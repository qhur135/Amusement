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

    //public int registerPlayer()
    //{
       
    //    int playerID = Random.Range(1, 10000);
    //    while (playerIDs.Contains(playerID))
    //    {
    //      playerID = Random.Range(1, 10000);
    //    }
    //    playerIDs.Add(playerID);

    //    print("playerid generate");

    //    return playerID;
        
    //}

    //public PhotonView getEnemy() // ?? ???
    //{
    //    int playercount = playerIDs.Count;
    //    print("player num:"+playercount);
    //    int randomidx = Random.Range(0, playercount);
    //    print("ramdomidx:"+randomidx);
    //    int enemyID = playerIDs[randomidx];
    //    print("enemyid:"+enemyID);
    //    GameObject [] players = GameObject.FindGameObjectsWithTag(RUNNER_TAG);
    //    print("player object num:" + players.Length);
    //    print("playerid[0]:" + playerIDs[0]);

    //    print("playerid[1]:" + playerIDs[1]);
    //    for (int i = 0; i < players.Length; i++)
    //    {
    //        Player p = players[i].GetComponent<Player>();
    //        print("playerid:" +i+" "+p.getPlayerID());
    //        if (p.isPlayerID(enemyID))
    //        {
    //            print("get pv");
    //            return p.getPV();
    //        }
    //    }

    //    return null;
    //}
    public void catchPlayer(Runner runner)
    {
        if(basePosition == Vector3.zero) 
        {
            basePosition = GameObject.FindWithTag(ENEMY_TAG).transform.position;
            runnerScale = runner.transform.lossyScale;
        }

        PhotonView PV = runner.getPV();
        this.PV.RPC(CAUGHT_RUNNER, RpcTarget.All, runner.getPlayerID());
        Vector3 nextPosition = new Vector3(basePosition.x + runnerScale.x * caughtRunners.Count, basePosition.y, basePosition.z);
        PV.RPC(CAUGHT_RUNNER, RpcTarget.All, nextPosition);
    }

    public void restartGame()
    {
        run();
        enemy();
        //PV.RPC("colorChangeToEnemy", RpcTarget.All);
       

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

            players[i].GetComponent<Runner>().enabled = true; // 스크립트 변경
            players[i].GetComponent<Enemy>().enabled = false;

            Runner runner = players[i].GetComponent<Runner>();
            runner.Awake();
            runner.Start();
        }

        Debug.Log("After runner setting");

    }

    [PunRPC]
    void enemySetting()
    {
        Debug.Log("Enemy setting start");

        GameObject enemy = GameObject.FindGameObjectWithTag(ENEMY_TAG);

        enemy.transform.position = new Vector3(1, 1.5f, 30);
        Debug.Log(enemy.transform.position);

        enemy.GetComponent<Enemy>().enabled = true;
        enemy.GetComponent<Runner>().enabled = false;

        Enemy newEnemy = enemy.GetComponent<Enemy>();
        newEnemy.Awake();
    }

    //[PunRPC]
    //void colorChangeToEnemy()
    //{
    //    GameObject enemy = GameObject.FindGameObjectWithTag(ENEMY_TAG);
    //    enemy.GetComponent<Renderer>().material.color = Color.red;
    //}

    [PunRPC]
    public void catchPlayer_RPC(int playerID)
    {
        caughtRunners.Add(playerID);
    }
}
