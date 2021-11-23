using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEnemyController : MonoBehaviour
{
    private enum State {
        Hit,
        SpikesOut,
        SpikesIn,
        IdleSpikes,
        Idle
    }

    private State currentState;

    private bool groundDetected;
    private bool wallDetected;

    [SerializeField] private float groundCheckDistance, wallCheckDistance;
    [SerializeField] private Transform groundCheck, wallCheck;
    private LayerMask whatIsGround;


    private void Update() {
        switch (currentState) {
            case State.Hit:
                UpdateHitState();
                break;
            case State.Idle:
                UpdateIdleState();
                break;
            case State.SpikesOut:
                UpdateSpikesOutState();
                break;
            case State.SpikesIn:
                UpdateSpikesInState();
                break;
            case State.IdleSpikes:
                UpdateIdleSpikesState();
                break;
        }       
    }

    //HIT
    private void EnterHitState() {

    }

    private void UpdateHitState() {

    }

    private void ExitHitState() {

    }
    //Spikes_out
    private void EnterSpikesOutState() {

    }

    private void UpdateSpikesOutState() {

    }

    private void ExitSpikesOutState() {

    }
    //Spikes_In
    private void EnterSpikesInState() {

    }

    private void UpdateSpikesInState() {

    }

    private void ExitSpikesInState() {

    }

    //IDLE
    private void EnterIdleState() {

    }

    private void UpdateIdleState() {

    }

    private void ExitIdleState() {

    }
    //IDLE Spikes

    private void EnterIdleSpikesState() {

    }

    private void UpdateIdleSpikesState() {

    }

    private void ExitIdleSpikesState() {

    }

    //Other stuff

    private void SwitchState(State state) {
        switch (currentState) {
            case State.Hit:
                ExitHitState();
                break;
            case State.Idle:
                ExitIdleState();
                break;
            case State.IdleSpikes:
                ExitIdleSpikesState();
                break;
            case State.SpikesIn:
                ExitSpikesInState();
                break;
            case State.SpikesOut:
                ExitSpikesOutState();
                break;
        }

        switch (state) {
            case State.Hit:
                EnterHitState();
                break;
            case State.Idle:
                EnterIdleState();
                break;
            case State.IdleSpikes:
                EnterIdleSpikesState();
                break;
            case State.SpikesIn:
                EnterSpikesInState();
                break;
            case State.SpikesOut:
                EnterSpikesOutState();
                break;
        }

        currentState = state;
    }
}
