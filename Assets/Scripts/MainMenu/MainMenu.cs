using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
        PlayerPrefs.DeleteAll();
        //mainMenuManager.GetAuth().SignOut();
        Application.Quit();
    }

    public void SwitchUser()
    {
        Debug.Log("SwitchUser");
        //mainMenuManager.GetAuth().SignOut();
        SceneManager.LoadScene(FinalValues.REGISTRATION_SCENE_INDEX);
    }
}


