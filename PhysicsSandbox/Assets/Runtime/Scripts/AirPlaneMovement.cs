using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirPlaneMovement : MonoBehaviour
{
    [Header("Physics")]
    //Velocidade para frente
    [SerializeField] private float thrust = 60;
    [SerializeField] private float yawTorque = 30;//Rotacao no eixo vertical ("y") do aviao
    [SerializeField] private float pitchTorque = 15;//rotacao no eixo direito ("x") do aviao

    [Header("RigidBody")]
    [SerializeField] private float mass = 10;
    [SerializeField] private float drag = 0.5f;
    [SerializeField] private float angularDrag = 10;

    [SerializeField]
    [Range(0, 80)]
    private float xRotClamp = 70;

    [Header("Visuals")]
    [SerializeField] private Transform graphics;
    [SerializeField] private float maxVisualRollAngle = 60;
    [SerializeField] private float visualRollAcc = 2;

    private Rigidbody rb;

    private float yawInput;
    private float pitchInput;

    private void Awake()
    {
        InitializeRigidBody();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateSteering();
    }
    private void UpdateMovement()
    {
        var moveForce = transform.forward * thrust;
        rb.AddForce(moveForce);
        /* rb.AddForce(moveForce, ForceMode.Acceleration); */
        /* rb.AddForce(moveForce * Time.fixedDeltaTime, ForceMode.Impulse); */
        /* rb.AddForce(moveForce * Time.fixedDeltaTime, ForceMode.VelocityChange); */

        /* float maxSpeed = 1.6f; */
        /* float acceleration = 2; */
        /* rb.velocity = Vector3.MoveTowards(rb.velocity, transform.forward * maxSpeed, Time.fixedDeltaTime * acceleration); */
    }

    private static float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    private void UpdateSteering()
    {
        var torque = new Vector3(
            pitchInput * pitchTorque,
            yawInput * yawTorque,
            0
        );

        rb.AddRelativeTorque(torque);

        var correctedRot = rb.rotation.eulerAngles;
        correctedRot.z = 0;
        correctedRot.x = ClampAngle(correctedRot.x, -xRotClamp, xRotClamp);
        rb.rotation = Quaternion.Euler(correctedRot);

        var graphicsLocalRotation = graphics.localEulerAngles;
        graphicsLocalRotation.z = Mathf.MoveTowardsAngle(graphicsLocalRotation.z, -maxVisualRollAngle * yawInput, Time.fixedDeltaTime * visualRollAcc);
        graphics.localEulerAngles = graphicsLocalRotation;
    }

    private void OnValidate()
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

    public void SetSteerInput(float pitch, float yaw)
    {
        pitchInput = pitch;
        yawInput = yaw;
    }

}
