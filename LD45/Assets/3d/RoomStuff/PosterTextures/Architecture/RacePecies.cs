using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacePecies : MonoBehaviour
{
    public GameObject StartPos;
    public GameObject EndPos;    
    public LayerMask playerLayer;
    public BoxCollider bc;    
    public Advertistments advertistments;
    public void SpawnNewPiece()
    {
        GameObject temp = Instantiate(Resources.Load("Groud") as GameObject, EndPos.transform.position, Quaternion.Euler(-90, 0, 0));
        temp.name = "Ground piece";     
        GameManager.instance.GroundTiles.Enqueue(temp);
        GameManager.instance.UpdateQueue();   
    }

    private void OnTriggerEnter(Collider coll)
    {
        print("bruh");
        if (coll.tag == "Player")
        {                     
            SpawnNewPiece();           
            bc.enabled = false;  
        }
    }
}
