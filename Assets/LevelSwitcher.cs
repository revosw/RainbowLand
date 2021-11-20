using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] string level;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(level);
    }
}
