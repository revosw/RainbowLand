using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D playerRB;
    private float maxVelocity;
    private Text text;
    void Start()
    {
       playerRB = GameObject.FindGameObjectWithTag("player").GetComponent<Rigidbody2D>();
       maxVelocity = playerRB.velocity.x;
       text = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.Backslash].wasPressedThisFrame)
        {
            maxVelocity = 0;
        }

        float currentVelocity = playerRB.velocity.x;
        if (Mathf.Abs(currentVelocity) > Mathf.Abs(maxVelocity))
        {
            maxVelocity = currentVelocity;
        }

        text.text = $"Max speed: {(maxVelocity)} ";
    }
}
