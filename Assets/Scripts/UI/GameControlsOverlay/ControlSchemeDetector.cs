using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class ControlSchemeDetector : MonoBehaviour
{

    public Image controllerIcon;

    public Image keyboardIcon;
    private string controlScheme;

    
    // Start is called before the first frame update
    void Awake()
    {
        controlScheme = "Keyboard&Mouse"; // Default to KB/M

        try
        {
            InputUser.onChange += InputUserOnChange; // Listen for scheme changes.

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controlScheme == "Gamepad")
        {
            // keyboardIcon.enabled = false;
            foreach (Transform child in keyboardIcon.transform)
            {
                child.gameObject.SetActive(false);
            }
            foreach (Transform child in controllerIcon.transform)
            {
                child.gameObject.SetActive(true);
            }
            // keyboardIcon.GetChild().gameObject.SetActive(false);
            // controllerIcon.enabled = true;
        }
        else  // all the fallbacks...
        {
            // keyboardIcon.enabled = true;
            foreach (Transform child in keyboardIcon.transform)
            {
                child.gameObject.SetActive(true);
            }
            foreach (Transform child in controllerIcon.transform)
            {
                child.gameObject.SetActive(false);
            }
            // keyboardIcon.GetComponentInChildren<Text>().enabled = true;
            // controllerIcon.enabled = false;
        }
        
    }

    private void InputUserOnChange(InputUser arg1, InputUserChange arg2, InputDevice arg3)
    {
        if (arg1.controlScheme.Value.name != null)
        {
            controlScheme = arg1.controlScheme.Value.name;
        }
        
    }
}
