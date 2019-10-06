using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardClick : MonoBehaviour
{
    public new AudioSource audio;
    public AudioClip ClickSound;

    public void Click()
    {
        audio.PlayOneShot(ClickSound);
    }
}
