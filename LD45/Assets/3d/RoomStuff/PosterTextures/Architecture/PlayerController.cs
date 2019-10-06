using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int index;
    public float speed;
    public float[] grid;
    public int gridIndex;
    public Rigidbody rb;

    // Change to make more effecient later
    public void Update()
    {
        if (gridIndex > 0 && Input.GetKeyDown(KeyCode.DownArrow))
        {
            gridIndex--;
            SwapLanes();
        }

        if (gridIndex < grid.Length-1 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            gridIndex++;                    
            SwapLanes();
        }

        speed = rb.velocity.magnitude;
      
        SwapLanes();

        // Set X velocity to 0
        Vector3 velocity = rb.velocity;
        velocity.x = 0;        
        rb.velocity = velocity;                 

        rb.AddForce(Time.deltaTime * 60 * Vector3.back * 5);
    }

    public void SwapLanes()
    {
        Vector3 temp = transform.position;
        temp.x = grid[gridIndex];
        transform.position = temp;
    }
}
