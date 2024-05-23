using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MENU : MonoBehaviour
{
    public bool GameIsPaused = false;
    public bool MenuHelpActive = false;
    public GameObject MENU_PAUSE;
    public GameObject MENU_HELP;
public perso_principal script;




    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
 public void Home()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void PlayGameMulti()
    {
        SceneManager.LoadSceneAsync(3);
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
    
    public void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            if (!MenuHelpActive)
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                } 
            }
            
        }
        
        
    }



public void Pause()
{
    
    MENU_PAUSE.SetActive(true);
    Time.timeScale = 0f;
    GameIsPaused = true;

}

public void Resume()
{

    MENU_PAUSE.SetActive(false);
    Time.timeScale = 1f;
    GameIsPaused = false;

}


public void HelpBoutton()
{
    MENU_PAUSE.SetActive(false);
    MENU_HELP.SetActive(true);
    MenuHelpActive = true;
}


public void NoHelpBoutton()
{
    MENU_HELP.SetActive(false);
    MENU_PAUSE.SetActive(true);
    
    MenuHelpActive = false;
}





}
