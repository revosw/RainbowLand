using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;

    //If this field is not filled, the door will be permanentely open.
    [SerializeField]
    public GameObject enemy;
    BoxCollider2D collider;
    bool notRemoteClosed = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public void openTheDoor() {
        notRemoteClosed = false;
        collider.enabled = false;
        animator.SetBool("DoorOpen", true);
    }
    
    public void closeTheDoor() {
        notRemoteClosed = true;
        collider.enabled = true;
        animator.SetBool("DoorOpen", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null && !notRemoteClosed) openTheDoor();
    }
}
