using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages pausing and resuming the game
/// </summary>
public class GameManager : MonoBehaviour
{
    [Tooltip("Needs the Canvas object holding main and pause menu.")]
    [SerializeField] UIManager uiManager;

    public void Exit()
    {
        Application.Quit();
    }

    public void OnPauseGame()
    {
        uiManager.OnPauseGame();
        Time.timeScale = 0;
    }

    public void OnResumeGame()
    {
        uiManager.OnResumeGame();
        Time.timeScale = 1;
    }
}