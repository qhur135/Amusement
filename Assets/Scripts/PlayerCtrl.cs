using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCtrl : MonoBehaviour
{
    public float speed = 3f;
    public float jumpPower = 30f;
    int jumpCount = 2;

    Vector3 movement;
    
    PhotonView view;

    private void Awake(){
        view = GetComponent<PhotonView>();
        if (!view.IsMine) 
        {
            Destroy(GetComponentInChildren<Camera>().gameObject); 
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (!view.IsMine) return; 
        transform.Translate(movement * Time.fixedDeltaTime * speed); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        jumpCount = 2; 
    }


}
