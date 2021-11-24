using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoorBossRoom : MonoBehaviour
{
    public Door door;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player") {
            door.closeTheDoor();
            print("Door closed!");
        } 
    }
}
