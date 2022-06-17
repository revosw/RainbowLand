using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    // Start is called before the first frame update
    public Door door;
    public GameObject enemy;

    // Update is called once per frame
    void Update()
    {
         if(enemy == null) door.openTheDoor();
        print("lmao");
        
    }
}
