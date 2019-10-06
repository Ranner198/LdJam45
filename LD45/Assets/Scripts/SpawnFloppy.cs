using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFloppy : MonoBehaviour
{
    public GameObject floppy;

    public void SpawnFloppyDisk()
    {
        GameObject temp = Instantiate(floppy, transform.position, Quaternion.Euler(-90, 0, 0));
        temp.GetComponent<Rigidbody>().velocity = Vector3.left;
    }
}
