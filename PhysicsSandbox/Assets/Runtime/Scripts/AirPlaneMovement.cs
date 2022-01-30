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

    [SerializeField] private float steeringSmoothSpeed = 1.5f;

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

    private Vector2 frameSteerInput;
    private Vector2 steerInput;

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

    private static float To180Angle(float angle)
    {
        angle = angle % 360;
        return angle > 180 ? angle - 360 : angle;
    }

    private void UpdateSteering()
    {
        steerInput = Vector2.Lerp(steerInput, frameSteerInput, Time.fixedDeltaTime * steeringSmoothSpeed);
        var torque = new Vector3(
            steerInput.x * pitchTorque,
            steerInput.y * yawTorque,
            0
        );

        var xRot = To180Angle(rb.rotation.eulerAngles.x);
        if ((frameSteerInput.x > 0 && xRot > xRotClamp) ||
            (frameSteerInput.x < 0 && xRot < -xRotClamp))
        {
            steerInput.x = 0;
            torque.x = 0;
        }

        rb.AddRelativeTorque(torque);

        //Force no Z rotation
        var correctedRot = rb.rotation.eulerAngles;
        correctedRot.z = 0;
        rb.rotation = Quaternion.Euler(correctedRot);

        var graphicsLocalRotation = graphics.localEulerAngles;
        graphicsLocalRotation.z = Mathf.MoveTowardsAngle(graphicsLocalRotation.z, -maxVisualRollAngle * steerInput.y, Time.fixedDeltaTime * visualRollAcc);
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

    public void SetSteerInput(Vector2 newFrameSteerInput)
    {
        frameSteerInput = newFrameSteerInput;
    }
}