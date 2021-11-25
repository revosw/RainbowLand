using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// When the player pauses the game, OnPauseGame
/// is implemented for all GameObjects that should
/// handle game pausing and resuming in a specific way.
/// </summary>
public interface IPausable
{
    // Start is called before the first frame update
    void OnPauseGame();
    void OnResumeGame();
}