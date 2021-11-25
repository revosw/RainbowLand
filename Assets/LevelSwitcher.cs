using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] GameObject blackBackground;
    bool shouldTransition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (name == "LevelEntrance")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void FixedUpdate()
    {
        if (shouldTransition)
        {

        }
    }
}
