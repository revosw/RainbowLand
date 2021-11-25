using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameMaster gm;
    public Animator animator;

    void Start() {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("player") || collision.gameObject.tag == "player") {
            GameMaster.lastCheckPointPosition = transform.position;
            //Animation for flag.
            animator.SetBool("isActive", true);
        }
    }
}
