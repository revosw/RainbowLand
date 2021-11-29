using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLevelExit : MonoBehaviour
{
    public GameObject bossMusic;
    public GameObject levelExit;

    private void OnDestroy()
    {
        bossMusic.SetActive(false);
        levelExit.SetActive(true);
    }
}
