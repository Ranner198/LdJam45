using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public GameObject target;
    public float distance, height, Z;
    
    void LateUpdate()
    {
        Vector3 targetPos = target.transform.position;        
        
        targetPos.x = distance;
        targetPos.y += height;
        targetPos.z += Z;

        transform.position = targetPos;
    }
}
