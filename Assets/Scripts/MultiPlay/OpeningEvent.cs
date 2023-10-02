using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class OpeningEvent : MonoBehaviourPun
{
    public Cinemachine.CinemachineVirtualCamera VirtualCamera;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject Sail_1p = PhotonNetwork.Instantiate("Sail_1p", new Vector3(-9, 0, 0), Quaternion.identity);
            VirtualCamera.Follow = Sail_1p.transform;
        }
        else
        {
            GameObject Sail_2p = PhotonNetwork.Instantiate("Sail_2p", new Vector3(9, 0, 0), Quaternion.identity);
            VirtualCamera.Follow = Sail_2p.transform;
        }
    }
}
