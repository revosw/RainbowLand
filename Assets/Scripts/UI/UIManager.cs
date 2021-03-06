using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IPausable
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject pauseMenu;

    public void OnPauseGame()
    {
        pauseMenu.SetActive(true);
    }

    public void OnResumeGame()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }
}
