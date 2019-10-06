using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfter : MonoBehaviour
{
    public float Delay;
    void Start()
    {
        StartCoroutine(Wait(Delay));
    }

    IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);

        DestroyImmediate(gameObject);
    }
}
