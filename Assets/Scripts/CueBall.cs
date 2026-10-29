using System;
using Unity.Cinemachine;
using UnityEngine;

public class CueBall : MonoBehaviour
{
    private Rigidbody rb;
    private CinemachineCamera camera;

    public float initialDrag = 0.5f;
    public float dragVelocity = 0.5f;
    public float minFollowVelocity = 0.3f;
    public int cameraPriority = 2;
    public float minCameraSwitchTime = 0.5f;

    private float lastCameraSwitchTime = float.NegativeInfinity;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        camera = GetComponentInChildren<CinemachineCamera>();

        rb.linearDamping = initialDrag;
    }

    void Update()
    {
        UpdateCamera();
        // UpdateDrag();
    }

    void UpdateDrag()
    {
        if (rb.linearVelocity.magnitude > 0f)
        {
            rb.linearDamping += dragVelocity * Time.deltaTime;
        }
        else
        {
            rb.linearDamping = initialDrag;
        }
    }

    void UpdateCamera()
    {
        if (Time.time - lastCameraSwitchTime >= minCameraSwitchTime)
        {
            camera.Priority = rb.linearVelocity.magnitude > minFollowVelocity ? cameraPriority : 0;
            lastCameraSwitchTime = Time.time;
        }
    }
}
