using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class SailMove_2p_MultiPlay : MonoBehaviourPun
{
    Rigidbody2D RB;
    SpriteRenderer SR;
    public GameObject Bullet_2p_prefab;
    public PhotonView PV;
    public UImanager_MultiPlay UImanager_MultiPlay;
    public Animator Animator;
    public bool Isinvincibility = false;
    public bool IsReCharge = false;
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
            ShootBullet();
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
        if (Input.GetKey(KeyCode.Space))
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

    public void ShootBullet()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !IsReCharge)
        {
            StartCoroutine(ReChargeShot());
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
        yield return new WaitForSeconds(2);
        Animator.SetBool("IsHitted", false);
        Isinvincibility = false;
    }

    IEnumerator ReChargeShot()
    {
        PV.RPC("ShootBullet_2p", RpcTarget.All);
        UImanager_MultiPlay.ReadyToFire.enabled = false;
        IsReCharge = true;
        yield return new WaitForSeconds(2f);
        UImanager_MultiPlay.ReadyToFire.enabled = true;
        IsReCharge = false;
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        SR.flipX = axis == -1;
    }

    [PunRPC]
    void ShootBullet_2p()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDirection = (mousePosition - (Vector2)transform.position).normalized;
        GameObject bullet = Instantiate(Bullet_2p_prefab, this.transform.position, this.transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * 6;
    }

}
