using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UImanager_SinglePlay : MonoBehaviour
{
    public TMP_Text Timer;
    public TMP_Text Score;
    public GameObject GameOverPanel;
    public GameObject RankingPanel;
    public TMP_Text[] RankingText;

    public int Time_min = 0;
    public float Time_sec = 0;
    public bool GameOver = false;

    // Update is called once per frame
    void Update()
    {
        if(!GameOver)
        {
            Time_sec += Time.deltaTime;
            if (Time_sec >= 59)
            {
                Time_sec = 0;
                Time_min++;
            }
            Timer.text = Time_min.ToString() + " : " + Time_sec.ToString("F0");
        }
        else if(GameOver)
        {
            GameEnd();
        }
    }

    public void GameEnd()
    {
        Score.text = "< Score [ " + Timer.text + " ] >";
        GameOverPanel.GetComponent<Animator>().SetTrigger("MovePanel");
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
        SceneManager.LoadScene("SinglePlayScene");
    }

    public void BackTitle()
    {
        SceneManager.LoadScene("StartScene");
    }
}
