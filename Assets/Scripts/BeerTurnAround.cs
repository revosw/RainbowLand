using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BeerTurnAround : MonoBehaviour
{
    //pathfinding
    public AIPath aipath;

    // Update is called once per frame
    void Update()
    {
        if(aipath.desiredVelocity.x >= .01f) {
            transform.localScale = new Vector3(10f, 10f, 1f);
        } else if (aipath.desiredVelocity.x <= -.01f){
            transform.localScale = new Vector3(-10f, 10f, 1f);
        }
    }
}
