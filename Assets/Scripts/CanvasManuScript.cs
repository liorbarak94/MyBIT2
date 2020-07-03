using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManuScript : MonoBehaviour
{
    public static bool gameIsPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
