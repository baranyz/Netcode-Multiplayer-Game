using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShipShake : NetworkBehaviour
{
    public float swingAngle = 10.0f;
    public float swingSpeed = 1.0f;

    private Quaternion originalRot;

    void Start()
    {
        originalRot = transform.localRotation;
    }

    void Update()
    {
        // Simulate mast swinging due to the sea
        float swing = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        Quaternion swingRot = Quaternion.Euler(swing, default, default);
        
       

        // Apply swinging motion to mast's local rotation
        transform.localRotation = originalRot * swingRot;
    }
}

