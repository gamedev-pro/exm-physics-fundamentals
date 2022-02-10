using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AirPlaneMovement))]
public class PlayerController : MonoBehaviour
{
    private AirPlaneMovement airPlaneMovement;
    /* private BombLauncher projectileLauncher; */

    private MachineGun machineGun;

    private void Awake()
    {
        airPlaneMovement = GetComponent<AirPlaneMovement>();
        /* projectileLauncher = GetComponent<BombLauncher>(); */
        machineGun = GetComponentInChildren<MachineGun>();
    }

    private void Update()
    {
        var frameInput = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        airPlaneMovement.SetSteerInput(frameInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            machineGun.StartShoot();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            machineGun.StopShoot();
        }
        /* if (Input.GetKey(KeyCode.Space)) */
        /* { */
        /*     projectileLauncher.TryShoot(); */
        /* } */
    }
}
