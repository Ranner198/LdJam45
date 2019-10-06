using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advertistments : MonoBehaviour
{
    public List<GameObject> adSigns = new List<GameObject>();
    public GameObject ad1, ad2;    
    private int previous;
    public void Start()
    {
        Quaternion rotation = Quaternion.Euler(-82.771f, -180, 90);
        previous = GenerateRandom(adSigns.Count-1);
        GameObject temp = Instantiate(adSigns[previous], ad1.transform.position, rotation);
        temp.transform.SetParent(transform);
        while (previous == GenerateRandom(adSigns.Count-1))
            GenerateRandom(adSigns.Count-1);
        temp = Instantiate(adSigns[previous], ad2.transform.position, rotation);
        temp.transform.SetParent(transform);
    }

    public int GenerateRandom(int max)
    {
        return Random.Range(0, max);
    }
}

