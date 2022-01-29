using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AirPlaneMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float steeringSmoothSpeed = 1.5f;

    private AirPlaneMovement airPlaneMovement;
    private Vector2 steerInput;

    private void Awake()
    {
        airPlaneMovement = GetComponent<AirPlaneMovement>();
    }

    private void Update()
    {
        var frameInput = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        steerInput = Vector2.Lerp(steerInput, frameInput, Time.deltaTime * steeringSmoothSpeed);
        airPlaneMovement.SetSteerInput(steerInput.x, steerInput.y);
    }
}
