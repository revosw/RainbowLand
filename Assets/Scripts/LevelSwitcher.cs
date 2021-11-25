using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] GameObject blackBackground;
    bool shouldTransition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AsyncOperation levelLoad;
        if (name == "LevelEntrance")
        {
            levelLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            levelLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }

        levelLoad.completed += (a) => GameObject.FindObjectOfType<PlayerController>().OnResumeGame();

        // if (name == "LevelEntrance")
        // {
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        // }
        // else
        // {
        //   SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // }
        // StartCoroutine(LoadLevelAsync());
        
    }

    // IEnumerator LoadLevelAsync()
    // {
    //
    //
    //     while (!levelLoad.isDone)
    //     {
    //         yield return null;
    //     
    //     }
    //
    //
    //
    // }

    private void FixedUpdate()
    {
        if (shouldTransition)
        {

        }
    }
}
