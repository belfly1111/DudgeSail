using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class BulletDispensor_MultiPlay : MonoBehaviourPun
{
    public GameObject bullet_prefab;
    public GameObject Player_1;
    public GameObject Player_2;

    public PhotonView PV;
    public float FireDelay = 3f;
    public float CurTime = 0;
    public int ShotPower = 3;

    public bool StartSpawn = false;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        PV.RPC("FindSail", RpcTarget.All);
    }

    // 1초 간격으로 총알 발사
    void Update()
    {
        if(StartSpawn)
        {
            CurTime += Time.deltaTime;
            if (CurTime > FireDelay)
            {
                PV.RPC("BulletDispenseRPC", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void BulletDispenseRPC()
    {
        GameObject bullet = Instantiate(bullet_prefab, this.transform.position, this.transform.rotation);

        int RN = Random.Range(0, 2);
        GameObject targetPlayer;
        if (RN == 1) targetPlayer = Player_1;
        else targetPlayer = Player_2;

        Vector2 direction = (targetPlayer.transform.position - this.transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * ShotPower;
        CurTime = 0;
    }

    [PunRPC]
    public void FindSail()
    {
        Player_1 = GameObject.Find("1p_Sail(Clone)");
        Player_2 = GameObject.Find("2p_Sail(Clone)");
        StartSpawn = true;
    }
}
