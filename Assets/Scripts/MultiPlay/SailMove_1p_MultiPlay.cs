using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class SailMove_1p_MultiPlay : MonoBehaviourPun
{
    Rigidbody2D RB;
    SpriteRenderer SR;
    public PhotonView PV;
    public UImanager_MultiPlay UImanager_MultiPlay;
    public Animator Animator;
    public bool Isinvincibility = false;
    public int PlayerSpeed = 3;
    public int life = 3;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        PV = GetComponent<PhotonView>();
        Animator = GetComponent<Animator>();
        UImanager_MultiPlay = GameObject.Find("UIManager_MultiPlay").GetComponent<UImanager_MultiPlay>();
    }

    void Update()
    {
        if (PV.IsMine)
        {
            UseBooster();
            playerMove();
        }
    }

    public void playerMove()
    {
        float axis_width = Input.GetAxisRaw("Horizontal");
        float axis_length = Input.GetAxisRaw("Vertical");

        if (axis_width != 0)
        {
            PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis_width);
        }

        RB.velocity = new Vector2(PlayerSpeed * axis_width, PlayerSpeed * axis_length);
    }

    public void UseBooster()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            UImanager_MultiPlay.slider.value -= Time.deltaTime;
            PlayerSpeed = 5;
        }
        else
        {
            UImanager_MultiPlay.slider.value += Time.deltaTime;
            PlayerSpeed = 3;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle" && !Isinvincibility && PV.IsMine)
        {
            UImanager_MultiPlay.DropHeart();
            StartCoroutine(invincibilityTime());
        }
    }

    IEnumerator invincibilityTime()
    {
        Animator.SetBool("IsHitted", true);
        Isinvincibility = true;
        yield return new WaitForSeconds(3f);
        Animator.SetBool("IsHitted", false);
        Isinvincibility = false;
    }


    [PunRPC]
    void FlipXRPC(float axis)
    {
        SR.flipX = axis == -1;
    }
}
