using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MENU : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void SettingsButton()
    {

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GoToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

public void Pause()
{
Time.timeScale = 0;

}

public void Resume()
{
Time.timeScale = 1;

}








}
