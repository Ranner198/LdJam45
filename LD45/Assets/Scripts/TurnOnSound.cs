using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnSound : MonoBehaviour
{
    public new AudioSource audio;
    public AudioClip PC;
    public bool isOn = false;

    public void PowerButton()
    {
        StopAllCoroutines();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);

        isOn = !isOn;

        if (isOn)
            audio.PlayOneShot(PC);
    }
}
