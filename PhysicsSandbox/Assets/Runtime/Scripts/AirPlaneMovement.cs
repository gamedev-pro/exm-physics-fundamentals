using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class AirPlaneMovement : MonoBehaviour
{
    [SerializeField] private float steerSmoothSpeed = 1.5f;

    [Header("Physics")]
    [SerializeField] private float thrust = 60;
    [SerializeField] private float pitchTorque = 30;
    [SerializeField] private float yawTorque = 15;
    [SerializeField] private float xRotClamp = 70;

    [Header("RigidBody")]

    [SerializeField] private float mass = 10;
    [SerializeField] private float drag = 0.5f;
    [SerializeField] private float angularDrag = 10;

    [Header("Visuals")]

    [SerializeField] private Transform graphics;
    [SerializeField] private float maxVisualRollAngle = 60;
    [SerializeField] private float visualRollAcc = 2;

    private Rigidbody rb;

    private Vector2 frameInput;
    private Vector2 steerInput;

    private void Awake()
    {
        InitializeRigidBody();
    }

    private void InitializeRigidBody()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        rb.drag = drag;
        rb.angularDrag = angularDrag;
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateSteering();
    }

    private static float To180Angle(float angle)
    {
        angle = angle % 360;
        if (angle > 180)
        {
            return angle - 360;
        }
        else if (angle < -180)
        {
            return angle + 360;
        }
        return angle;
    }

    private void UpdateSteering()
    {
        steerInput = Vector2.Lerp(steerInput, frameInput, Time.fixedDeltaTime * steerSmoothSpeed);
        var torque = new Vector3(
            steerInput.x * pitchTorque,
            steerInput.y * yawTorque,
            0
        );

        if (ShouldBlockPitch())
        {
            steerInput.x = 0;
            torque.x = 0;
        }

        rb.AddRelativeTorque(torque);

        //Foce no Z rotation
        var correctedRot = rb.rotation.eulerAngles;
        correctedRot.z = 0;
        rb.rotation = Quaternion.Euler(correctedRot);

        //Graphics
        var graphicsLocalRotation = graphics.localEulerAngles;
        var targetZ = -maxVisualRollAngle * steerInput.y;
        graphicsLocalRotation.z = Mathf.MoveTowardsAngle(graphicsLocalRotation.z, targetZ, Time.fixedDeltaTime * visualRollAcc);
        graphics.localEulerAngles = graphicsLocalRotation;
    }

    private bool ShouldBlockPitch()
    {
        var xRot = To180Angle(rb.rotation.eulerAngles.x);
        // Se a gente ja passou do limite E esta tentando mover mais naquela direcao, bloqueia o movimento
        return (frameInput.x > 0 && xRot > xRotClamp) ||
                    (frameInput.x < 0 && xRot < -xRotClamp);
    }

    private void UpdateMovement()
    {
        var moveForce = transform.forward * thrust;
        rb.AddForce(moveForce);
    }

    public void SetSteerInput(Vector2 newFrameInput)
    {
        frameInput = newFrameInput;
    }
}
