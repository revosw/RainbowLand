using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossMusic : MonoBehaviour
{
    private AudioClip bossStart;
    public GameObject bossLoop;
    void Start()
    {
        bossStart = GetComponent<AudioSource>().clip;
        StartCoroutine(playBossLoop());
    }

    IEnumerator playBossLoop()
    {
        yield return new WaitForSeconds(bossStart.length);
        bossLoop.SetActive(true);
    }
}
