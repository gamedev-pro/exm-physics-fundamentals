using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AirPlaneMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private ROFWeapon weapon;
    private AirPlaneMovement airPlaneMovement;

    private void Awake()
    {
        airPlaneMovement = GetComponent<AirPlaneMovement>();
    }

    private void Update()
    {
        var frameInput = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        airPlaneMovement.SetSteerInput(frameInput);

        if (weapon != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                weapon.StartShoot();
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                weapon.StopShoot();
            }
        }

    }
}
