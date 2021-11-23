using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;

    public bool doorOpen { set; get; }

    // Start is called before the first frame update
    void Start()
    {
        doorOpen = false;
    }

    public void openTheDoor() {
        animator.SetBool("DoorOpen", true);
        GetComponent<BoxCollider2D>().enabled = false;
    }
    
    public void closeTheDoor() {
        animator.SetBool("DoorOpen", false);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
