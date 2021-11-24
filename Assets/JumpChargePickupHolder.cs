using System.Collections;
using System.Collections.Generic;
using Pickups;
using Player;
using UnityEngine;

public class JumpChargePickupHolder : MonoBehaviour
{
    public DoubleJumpPickup[] pickups;

    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded)
        {
            foreach (var doubleJumpPickup in pickups)
            {
                doubleJumpPickup.SetActiveState(true);
            }
        }
    }
}