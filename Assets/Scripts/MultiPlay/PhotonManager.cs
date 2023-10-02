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

    // '1' �̶�� MasterPlayer(����) '0' �̶�� Player(�Ķ�)
    public int PlayerRole = -1;

    // �̱��� ������ �̿��� ���� ���� Scene���� �����ϴ� �پ��� stage�� �Ѿ�� Pun2 ���� ������ ������ �� �ֵ��� �Ѵ�.
    public static PhotonManager instance;


    void Awake()
    {
        Application.targetFrameRate = 144;
        PhotonNetwork.AutomaticallySyncScene = true;

        // -------------------------------------------
        PhotonNetwork.SendRate = 144;
        PhotonNetwork.SerializationRate = 144;
        // -------------------------------------------


        // PhotonManeger �̱��� ó��
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �ٸ� Scene���� PhotonManager�� �̹� �ִ��� Ȯ���ϰ� �ִٸ� �ı����� �ʽ��ϴ�.
            PhotonManager[] managers = FindObjectsOfType<PhotonManager>();
            if (managers.Length > 1)
            {
                // �� �� �̻��� �ν��Ͻ��� �ִ� ���, ù ��°�� ������ ��� �ٸ� �ν��Ͻ��� �ı��մϴ�.
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
            // Photon ������ ����
            PhotonNetwork.ConnectUsingSettings();
        }
        // ���ӽ��� �� console�� ���и޼��� ����.
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to connect to Photon Server: " + ex.Message);
            RoomStatus.text = "CurState : Connect Failed. Please Restart Game.";
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon Server ���� �Ϸ�");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Photon Server Lobby ���� �Ϸ�");
        RoomStatus.text = "CurState : Complete Connect Server! Make Room Or Join Room!";
        MakeRoom_Button.SetActive(true);
        JoinRoom_Button.SetActive(true);
    }
    public void CreateRoom()
    {
        // ����ڰ� ���̸��� �Է����� ���� ���� ���������.
        if (Make_RoomName.text != "")
        {
            // RoomOptions. �÷��̾�� ������ �� ���� ���;� �ϰ� ���� �������� �ʴ´�.
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.EmptyRoomTtl = 0; // ���� ������� ���� ���� �ð� (0���� �����ϸ� ��� ����)
            roomOptions.IsVisible = false;

            // ��й�ȣ�� �Ǻ��� �ϱ� ���� �������. - �� ��, ���̸� ��ü�� ��й�ȣ ������ �Ѵ�.
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
        print("PhotonServer �游��� �Ϸ�");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("PhotonServer �游��� ����");
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

    // �÷��̾ room���� ������ �� ȣ��Ǵ� �Լ�. 5�ʰ� ������ ������ ����ǰ� ���� ó���ؾ��Ѵ�.
    // ���� ���ο����� ������ ó���ؾ��Ѵ�.
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
        Debug.Log("���� �����ϴ�. : ");
        PhotonNetwork.LeaveRoom();
    }
    public void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Photon Server ���� ����");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.PlayerList.Length == 2 && PhotonNetwork.IsMasterClient)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("������ �濡 ���� : " + newPlayer);
            GameStart_Button.SetActive(true);
        }
    }
}
