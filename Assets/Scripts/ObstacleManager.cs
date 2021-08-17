using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    //public GameObject lotaionX_Prefab;
    public GameObject pendulum_Prefab;

    PhotonView PV;

    // Start is called before the first frame update
    private void Awake()
    {
        //lotaionX_Prefab.transform.rotation = Quaternion.Euler(90, 0, 0);
        PV = GetComponent<PhotonView>();
        
    }
    void Start()
    {
        //Vector3 lotaion_pos = new Vector3(-3, 1, 4);
        //PhotonNetwork.Instantiate(lotaionX_Prefab.name, lotaion_pos, Quaternion.identity);
       
        Vector3 pendul_pos = new Vector3(0, 9, 18);
        //PhotonNetwork.Instantiate(pendulum_Prefab.name, pendul_pos, Quaternion.identity);
        print("obstacle generate");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
