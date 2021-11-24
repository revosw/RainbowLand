using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;

    //If this field is not filled, the door will be permanentely open.
    [SerializeField]
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
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
        if (enemy == null) openTheDoor();
    }
}
