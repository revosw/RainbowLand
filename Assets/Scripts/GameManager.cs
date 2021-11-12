using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages pausing and resuming the game
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    public void Exit()
    {
        Application.Quit();
        Debug.Log("NDOQWODWQJJ");
    }

    public void OnPauseGame()
    {
        uiManager.OnPauseGame();
    }

    public void OnResumeGame()
    {
        uiManager.OnResumeGame();
    }
}