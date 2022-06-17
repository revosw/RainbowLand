using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
    public static Vector2 lastCheckPointPosition;
    private Transform player;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        // set checkpoint position to player starting position.
        lastCheckPointPosition = player.position;
    }
}