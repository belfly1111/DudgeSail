using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class GameEvent : MonoBehaviourPun
{
    public Cinemachine.CinemachineVirtualCamera VirtualCamera;
    public GameObject BulletDispensers;
    public GameObject Shark;

    public float curTime = 0;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject Sail_1p = PhotonNetwork.Instantiate("1p_Sail", new Vector3(-9, 0, 0), Quaternion.identity);
            VirtualCamera.Follow = Sail_1p.transform;
            PhotonManager.instance.PlayerRole = 1;
        }
        else
        {
            GameObject Sail_2p = PhotonNetwork.Instantiate("2p_Sail", new Vector3(9, 0, 0), Quaternion.identity);
            VirtualCamera.Follow = Sail_2p.transform;
            PhotonManager.instance.PlayerRole = 0;
        }
        BulletDispensers.SetActive(true);
    }
}
