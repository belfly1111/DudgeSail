using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEditor;


public class UImanager_title : MonoBehaviourPun
{
    public GameObject TutorialPanel;
    public GameObject RankingPanel;

    #region StartScene
    public void playSingleGame()
    {
        SceneManager.LoadScene("SinglePlayScene");
    }

    public void playMultiGame()
    {
        SceneManager.LoadScene("MultiPlayLobbyScene");
    }

    public void Ranking()
    {

    }
    #endregion

    #region MultiPlayLobbyScene

    public void GameStart_MultiPlay()
    {
        PhotonNetwork.LoadLevel("MultiPlayScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackTitle()
    {
        PhotonNetwork.Disconnect();
        Destroy(PhotonManager.instance.gameObject);
        SceneManager.LoadScene("StartScene");
    }

    public void HowToPlay()
    {

    }
    #endregion
}
