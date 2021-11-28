using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//todo: move to correct location in Assets when done

public class ControlsUIOverlay : MonoBehaviour
{

    private Text text;

    private PlayerInputAction playerInput;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        playerInput = player.GetComponent<PlayerController>().GetPlayerInputInstance();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        var anyPress = InputSystem.onAnyButtonPress.GetType();
        var devices = InputSystem.devices;
        // Debug.Log($"InputSystem.devices = {devices}");
        string devs = "";
        foreach (var dev in devices)
        {
            devs = devs + $"{dev.description.deviceClass}, ";
        }
        
        
        text.text = $"Active controller: {devs} ";

    }

}
