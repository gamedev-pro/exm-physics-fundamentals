using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AirPlaneMovement))]
public class PlayerController : MonoBehaviour
{
    private AirPlaneMovement airPlaneMovement;

    private void Awake()
    {
        airPlaneMovement = GetComponent<AirPlaneMovement>();
    }

    private void Update()
    {
        var frameInput = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        airPlaneMovement.SetSteerInput(frameInput);
    }
}
