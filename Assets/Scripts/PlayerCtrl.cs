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
        if (!view.IsMine) // �÷��̾�� �ֳʹ̿� ī�޶� ��� �پ��ִ� ���¿���(ī�޶� 4�� ����)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject); // ������ �ƴϸ� ī�޶� �ı� -> ī�޶� 2�� ����
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (!view.IsMine) return; // ���� �ƴϸ� �̵����� �ʱ�
        transform.Translate(movement * Time.fixedDeltaTime * speed); // ����� �̵��ϱ�
    }

    private void OnCollisionEnter(Collision collision)
    {
        jumpCount = 2; 
    }


}
