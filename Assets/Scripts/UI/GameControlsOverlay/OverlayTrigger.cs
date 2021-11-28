using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTrigger : MonoBehaviour
{

    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
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
