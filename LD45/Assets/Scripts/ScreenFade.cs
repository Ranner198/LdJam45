using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFade : MonoBehaviour
{
    public Image fade;

    public bool delay = false;

    public void Awake()
    {
        fade.gameObject.SetActive(true);
    }
    public void Start()
    {
        if (delay)
            StartCoroutine(Fade(0, 1.3f, "", 2));
        else
            StartCoroutine(Fade(0, 1.3f, ""));
    }
    public void FinishScene()
    {
        StartCoroutine(Fade(1, 1.3f, "Credits"));
    }
    public void FinishScene(string name)
    {
        StartCoroutine(Fade(1, 1.3f, name));
    }
    IEnumerator Fade(float amt, float delay, string nextScene)
    {        
        fade.CrossFadeAlpha(amt, delay, false);
        yield return new WaitForSeconds(delay);
        if (nextScene.Length > 0)
            SceneManager.LoadScene(nextScene);
    }
    IEnumerator Fade(float amt, float delay, string nextScene, float delayamt)
    {
        yield return new WaitForSeconds(2);
        fade.CrossFadeAlpha(amt, delay, false);
        yield return new WaitForSeconds(delay);
        if (nextScene.Length > 0)
            SceneManager.LoadScene(nextScene);
    }
}
