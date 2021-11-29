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
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player")) canvas.SetActive(true);

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("player")) canvas.SetActive(false);
    }
}
