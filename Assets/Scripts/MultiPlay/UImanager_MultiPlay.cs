using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;

public class UImanager_MultiPlay : MonoBehaviourPun
{
    public GameObject GameOverPanel;
    public GameObject RankingPanel;

    public PhotonView PV;
    public TMP_Text GameResult;
    public TMP_Text Timer;
    public TMP_Text Score;
    public TMP_Text[] RankingText;

    public Image[] Heart_1p;
    public Image[] Heart_2p;

    public int HeartCount_1p = 0;
    public int HeartCount_2p = 0;

    public int Time_min = 0;
    public float Time_sec = 0;
    public bool GameOver = false;


    // Update is called once per frame
    void Update()
    {
        if (!GameOver)
        {
            Time_sec += Time.deltaTime;
            if (Time_sec >= 59)
            {
                Time_sec = 0;
                Time_min++;
            }
            Timer.text = Time_min.ToString() + " : " + Time_sec.ToString("F0");
        }
    }

    public void Ranking()
    {
        Debug.Log("·©Å· º¸±â");
        if(RankingPanel.activeSelf)
        {
            RankingPanel.SetActive(false);
        }
        else
        {
            RankingPanel.SetActive(true);
        }
    }

    public void ReStart()
    {
        PV.RPC("ReStartMultiPlay", RpcTarget.All);
    }

    public void BackTitle()
    {
        PhotonNetwork.Disconnect();
        Destroy(PhotonManager.instance.gameObject);
        SceneManager.LoadScene("StartScene");
    }

    public void DropHeart()
    {
        if(HeartCount_1p == 3 || HeartCount_2p == 3)
        {
            PV.RPC("GameEnd", RpcTarget.All);
        }

        // MasterPlayer == »¡°­
        else if(PhotonManager.instance.PlayerRole == 1)
        {
            PV.RPC("DropHeart_1p", RpcTarget.All);
        }
        // Player == ÆÄ¶û
        else if(PhotonManager.instance.PlayerRole == 0)
        {
            PV.RPC("DropHeart_2p", RpcTarget.All);
        }
    }

    [PunRPC]
    public void DropHeart_1p()
    {
        Destroy(Heart_1p[HeartCount_1p]);
        HeartCount_1p++;
    }

    [PunRPC]
    public void DropHeart_2p()
    {
        Destroy(Heart_2p[HeartCount_2p]);
        HeartCount_2p++;
    }

    [PunRPC]
    public void GameEnd()
    {
        Score.text = "< Score [ " + Timer.text + " ] >";

        if (HeartCount_1p == 3) GameResult.text = "GameOver - 2P WINS!";
        else GameResult.text = "GameOver - 1P WINS!";
        
        GameOverPanel.GetComponent<Animator>().SetTrigger("MovePanel");
        GameOver = true;
    }

    [PunRPC]
    public void ReStartMultiPlay()
    {
        PhotonNetwork.LoadLevel("MultiPlayScene");
    }

}
