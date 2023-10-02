using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class SailMove_MultiPlay : MonoBehaviourPun
{
    Rigidbody2D RB;
    SpriteRenderer SR;
    public PhotonView PV;
    public UImanager_MultiPlay UImanager_MultiPlay;
    public int life = 3;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        PV = GetComponent<PhotonView>();
        UImanager_MultiPlay = GameObject.Find("UIManager_MultiPlay").GetComponent<UImanager_MultiPlay>();
    }

    void Update()
    {
        if(PV.IsMine) playerMove();
    }

    public void playerMove()
    {
        float axis_width = Input.GetAxisRaw("Horizontal");
        float axis_length = Input.GetAxisRaw("Vertical");

        if (axis_width != 0)
        {
            PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis_width);
        }

        RB.velocity = new Vector2(3 * axis_width, 3 * axis_length);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle" && PV.IsMine)
        {
            UImanager_MultiPlay.DropHeart();
        }
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        SR.flipX = axis == -1;
    }
}
