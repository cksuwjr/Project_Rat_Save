using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//By Jasmine
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if(pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    // public void Pause()
    // {
    //     if(pauseMenu.activeSelf)
    //     {
    //         pauseMenu.SetActive(true);
    //         Time.timeScale = 0;
    //     }
    //     else
    //     {
    //         pauseMenu.SetActive(false);
    //         Time.timeScale = 1;
    //     }
    // }

    // public void Resume()
    // {
    //     pauseMenu.SetActive(false);
    //     Time.timeScale = 1;
    // }

    public void MuteToggle(bool muted)
    {
        if (muted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }
}
