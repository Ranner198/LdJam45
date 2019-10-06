using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{    
    public Queue<GameObject> GroundTiles = new Queue<GameObject>();
    public static GameManager instance;
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    void Update()
    {
        
    }

    public void UpdateQueue()
    {
        if(GroundTiles.Count > 4)
        {
            Destroy(GroundTiles.Peek());
            GroundTiles.Dequeue();
        }
    }
}