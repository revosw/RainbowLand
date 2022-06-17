using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public Door door;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player") {
            if (name == "WaypointCloseDoor")
            {
                door.closeTheDoor();
                print("Door closed!");
            }
            else
            {
                door.openTheDoor();
            }
        } 
    }
}
