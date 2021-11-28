using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] GameObject blackBackground;
    [SerializeField] GameObject banner;
    [SerializeField] AnimationCurve bannerMovement;
    Image backgroundColor;

    private void Awake()
    {
        backgroundColor = blackBackground.GetComponent<Image>();
        
    }

    private void Start()
    {
        if (banner != null)
        {
            backgroundColor.CrossFadeAlphaWithCallBack(0, 1f, () => StartCoroutine(ShowBanner()));
        }
    }

    IEnumerator ShowBanner()
    {
        float time = 0;
        while (time < bannerMovement.keys[bannerMovement.keys.Length-1].time)
        {
            time += Time.deltaTime;

            banner.transform.localPosition = new Vector3(0, bannerMovement.Evaluate(time), 0);

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CompareTag("player")) return;

        backgroundColor.CrossFadeAlphaWithCallBack(1f, 1f, delegate
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

            levelLoad.completed += (a) =>
            {
                FindObjectOfType<PlayerController>().OnResumeGame();
            };
        });

        
        

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

    //IEnumerator FadeToBlack()
    //{
    //    backgroundColor.CrossFadeAlpha(255f, 1f, true);
    //    yield return new WaitForSeconds(1f);
        
    //}
}
