using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public TMPro.TMP_Text RoomStatus;
    public TMPro.TMP_InputField Make_RoomName;
    public TMPro.TMP_InputField Join_RoomName;

    public GameObject MakeRoom_Button;
    public GameObject JoinRoom_Button;
    public GameObject GameStart_Button;

    // '1' 이라면 MasterPlayer(빨강) '0' 이라면 Player(파랑)
    public int PlayerRole = -1;

    // 싱글톤 패턴을 이용해 여러 개의 Scene으로 관리하는 다양한 stage로 넘어가도 Pun2 서버 연결을 유지할 수 있도록 한다.
    public static PhotonManager instance;


    void Awake()
    {
        Application.targetFrameRate = 144;
        PhotonNetwork.AutomaticallySyncScene = true;

        // -------------------------------------------
        PhotonNetwork.SendRate = 144;
        PhotonNetwork.SerializationRate = 144;
        // -------------------------------------------


        // PhotonManeger 싱글톤 처리
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 다른 Scene에서 PhotonManager가 이미 있는지 확인하고 있다면 파괴하지 않습니다.
            PhotonManager[] managers = FindObjectsOfType<PhotonManager>();
            if (managers.Length > 1)
            {
                // 두 개 이상의 인스턴스가 있는 경우, 첫 번째를 제외한 모든 다른 인스턴스를 파괴합니다.
                for (int i = 1; i < managers.Length; i++)
                {
                    Destroy(managers[i].gameObject);
                }
            }
        }
        ConnectPhoton();
    }

    public void ConnectPhoton()
    {
        try
        {
            // Photon 서버에 접속
            PhotonNetwork.ConnectUsingSettings();
        }
        // 접속실패 시 console에 실패메세지 남김.
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to connect to Photon Server: " + ex.Message);
            RoomStatus.text = "CurState : Connect Failed. Please Restart Game.";
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon Server 접속 완료");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Photon Server Lobby 접속 완료");
        RoomStatus.text = "CurState : Complete Connect Server! Make Room Or Join Room!";
        MakeRoom_Button.SetActive(true);
        JoinRoom_Button.SetActive(true);
    }
    public void CreateRoom()
    {
        // 사용자가 방이름을 입력했을 때만 방이 만들어진다.
        if (Make_RoomName.text != "")
        {
            // RoomOptions. 플레이어는 무조건 두 명이 들어와야 하고 방은 공개하지 않는다.
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.EmptyRoomTtl = 0; // 방이 비어있을 때의 생존 시간 (0으로 설정하면 즉시 삭제)
            roomOptions.IsVisible = false;

            // 비밀번호로 판별을 하기 위해 만들어줌. - 이 때, 방이름 자체가 비밀번호 역할을 한다.
            PhotonNetwork.CreateRoom(Make_RoomName.text, roomOptions);
            RoomStatus.text = "CurState : Sucess Make Room. Waiting your Friends...";
        }
        else
        {
            RoomStatus.text = "CurState : When you make the Room, Must input RoomName";
        }
    }
    public override void OnCreatedRoom()
    {
        MakeRoom_Button.SetActive(false);
        JoinRoom_Button.SetActive(false);
        print("PhotonServer 방만들기 완료");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("PhotonServer 방만들기 실패");
        RoomStatus.text = "CurState : Failed to make room. The same room name may already exist. Or restart.";
    }

    public void JoinRoom()
    {
        if(Join_RoomName.text != "")
        {
            PhotonNetwork.JoinRoom(Join_RoomName.text);
        }
        else
        {
            RoomStatus.text = "CurState : When you Join the Room, Must input RoomName";
        }
    }
    public override void OnJoinedRoom()
    {
        MakeRoom_Button.SetActive(false);
        JoinRoom_Button.SetActive(false);
        RoomStatus.text = "CurState : Make / Join Room Sucessful. Wait for it to Start!";
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        RoomStatus.text = "CurState : Failed to join room. Please re-enter the correct room name.";
    }

    // 플레이어가 room에서 나갔을 때 호출되는 함수. 5초가 지나면 게임이 종료되게 추후 처리해야한다.
    // 게임 내부에서는 적절히 처리해야한다.
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            LeaveRoom();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("StartScene");
            Destroy(PhotonManager.instance.gameObject);
        }
    }
    public void LeaveRoom()
    {
        Debug.Log("방을 떠납니다. : ");
        PhotonNetwork.LeaveRoom();
    }
    public void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Photon Server 접속 끊김");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.PlayerList.Length == 2 && PhotonNetwork.IsMasterClient)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("누군가 방에 들어옴 : " + newPlayer);
            GameStart_Button.SetActive(true);
        }
    }
}
