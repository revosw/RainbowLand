using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

//todo: move to correct location in Assets when done

public class ControlsUIOverlay : MonoBehaviour
{
    private Text text;
    private PlayerInputAction playerInputAction;


    private string controlScheme;
    private string inputDeviceName;

    public GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        controlScheme = "Keyboard&Mouse"; // Default to KB/M

        InputUser.onChange += InputUserOnChange;

        
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        // InputUser.onChange += InputUserOnChange;
        text.text = $"Active controller: {controlScheme} ";
    }

    private void InputUserOnChange(InputUser arg1, InputUserChange arg2, InputDevice arg3)
    {
        Debug.Log("InputUser change callback.");
        // if (arg3 != null)
        // {
        //     inputDeviceName = arg3.displayName;
        // }
        controlScheme = arg1.controlScheme.Value.name;
        // inputDeviceName = arg3.name;
    }
}