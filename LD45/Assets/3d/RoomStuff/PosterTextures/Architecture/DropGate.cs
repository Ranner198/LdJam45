using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGate : MonoBehaviour
{
    public float countDownTimer;
    public Animator anim;
    public BoxCollider bc;
    void Start()
    {
        StartCoroutine(CountDown());
    }
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(countDownTimer);

        anim.Play("StartAnimation");

        bc.enabled = false;
    }
}
