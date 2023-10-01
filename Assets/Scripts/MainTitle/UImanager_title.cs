using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager_title : MonoBehaviour
{

    public void playSingleGame()
    {
        SceneManager.LoadScene("SinglePlayScene");
    }

    public void playMultiGame()
    {

    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
