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
        if (!view.IsMine) // 플레이어와 애너미에 카메라 모두 붙어있는 상태에서(카메라 4개 생성)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject); // 내꺼가 아니면 카메라 파괴 -> 카메라 2개 남음
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (!view.IsMine) return; // 내가 아니면 이동하지 않기
        transform.Translate(movement * Time.fixedDeltaTime * speed); // 나라면 이동하기
    }

    private void OnCollisionEnter(Collision collision)
    {
        jumpCount = 2; 
    }


}
