using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class ControlSchemeDetector : MonoBehaviour
{

    public Sprite controllerIcon;

    public Sprite keyboardIcon;
    
    
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
}
