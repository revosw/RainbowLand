using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
    private static GameMaster instance;
    public Vector2 lastCheckPointPosition;
    private Transform player;

    private void Awake() {
        if (instance == null) {
            instance = this;
            player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
            // set checkpoint position to player starting position.
            lastCheckPointPosition = player.position;
            
            DontDestroyOnLoad(instance);
        } else {
            Destroy(gameObject);
        }
    }
}