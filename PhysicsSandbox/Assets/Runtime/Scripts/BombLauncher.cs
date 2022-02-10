using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLauncher : ROFWeapon
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private Bomb bombPrefab;

    [SerializeField] private float shootForce = 10;

    protected override void Shoot()
    {
        var bomb = Instantiate(bombPrefab, muzzle.position, Quaternion.LookRotation(muzzle.forward));
        bomb.GetComponent<Rigidbody>().AddForce(muzzle.forward * shootForce, ForceMode.Impulse);
    }
}
