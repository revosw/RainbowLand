using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLevelExit : MonoBehaviour
{
    public GameObject levelExit;

    private void OnDestroy()
    {
        levelExit.SetActive(true);
    }
}
