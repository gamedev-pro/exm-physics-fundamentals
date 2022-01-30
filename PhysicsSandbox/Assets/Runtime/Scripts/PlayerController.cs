using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AirPlaneMovement), typeof(BombLauncher))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float steeringSmoothSpeed = 1.5f;

    private AirPlaneMovement airPlaneMovement;
    private BombLauncher projectileLauncher;

    private void Awake()
    {
        airPlaneMovement = GetComponent<AirPlaneMovement>();
        projectileLauncher = GetComponent<BombLauncher>();
    }

    private void Update()
    {
        var frameInput = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        airPlaneMovement.SetSteerInput(frameInput);

        if (Input.GetKey(KeyCode.Space))
        {
            projectileLauncher.TryShoot();
        }
    }
}
