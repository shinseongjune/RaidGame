using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("BossBattleScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
