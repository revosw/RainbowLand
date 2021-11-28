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

    private string controlScheme;
    private string inputDeviceName;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
        // var devices = InputSystem.devices;
        // // Debug.Log($"InputSystem.devices = {devices}");
        // string devs = "";
        // foreach (var dev in devices)
        // {
        //     devs = devs + $"{dev.description.deviceClass}, ";
        // }
    
        InputUser.onChange += InputUserOnChange;

        
        text.text = $"Active controller: {inputDeviceName} ";

    }
    
    private void InputUserOnChange(InputUser arg1, InputUserChange arg2, InputDevice arg3)
    {
        // if (arg3 != null)
        // {
        //     inputDeviceName = arg3.displayName;
        // }
        controlScheme = arg1.controlScheme.ToString();
        // inputDeviceName = arg3.name;
    }

}
