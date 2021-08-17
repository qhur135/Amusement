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
        //this.PV.RPC(CAUGHT_RUNNER, RpcTarget.All, runner.getPlayerID()); // 잡힌 애들 저장하기
        Vector3 nextPosition = new Vector3(basePosition.x + runnerScale.x * caughtRunners.Count, basePosition.y, basePosition.z);
        PV.RPC(CAUGHT_RUNNER, RpcTarget.All, nextPosition); // 술래 옆에 붙잡아 놓기 - 러너 코드에 rpc함수있음
    }

    
    [PunRPC]
    public void catchPlayer_RPC(int playerID)
    {
        caughtRunners.Add(playerID);
    }
}
