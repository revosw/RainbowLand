using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class OverlayTrigger : MonoBehaviour
{

    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InputUser.onChange += InputUserOnChange;
    }

    private void InputUserOnChange(InputUser arg1, InputUserChange arg2, InputDevice arg3)
    {
        arg1.controlScheme.ToString();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Canvas Active");
        canvas.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Canvas Inactive");
        canvas.SetActive(false);
    }
}
