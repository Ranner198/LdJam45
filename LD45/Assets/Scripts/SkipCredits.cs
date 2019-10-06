using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCredits : MonoBehaviour
{
    public float autoloadTimer = 20;

    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            //ScreenFade.instance.FinishScene("MainMenu");
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(autoloadTimer);
        GameObject.FindObjectOfType<ScreenFade>().FinishScene("MainMenu");
    } 
}
