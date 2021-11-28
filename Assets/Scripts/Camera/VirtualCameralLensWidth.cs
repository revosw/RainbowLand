using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player;
using UnityEngine;

public class VirtualCameralLensWidth : MonoBehaviour
{

    public AnimationCurve lensCurve;

    private Rigidbody2D RB;
    CinemachineVirtualCamera camera;

    // Start is called before the first frame update
    void Start()
    {
        RB = GameObject.FindObjectOfType<PlayerController>().GetComponentInChildren<Rigidbody2D>();
        camera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {

        // float lensEval = lensCurve.Evaluate(playerYVelocity);
        camera.m_Lens.OrthographicSize = lensCurve.Evaluate(Mathf.Abs(RB.velocity.y));
    }
}
