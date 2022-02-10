using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLauncher : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private Bomb bombPrefab;

    [SerializeField] private float shootForce = 10;

    //Rate of fire: numero de tiros por segundo
    [SerializeField] private float rof = 2;

    private float ShootDelay => 1.0f / rof;
    private float nextShootTime;
    public void TryShoot()
    {
        if (Time.time > nextShootTime)
        {
            nextShootTime = Time.time + ShootDelay;
            var bomb = Instantiate(bombPrefab, muzzle.position, Quaternion.LookRotation(muzzle.forward));
            bomb.GetComponent<Rigidbody>().AddForce(muzzle.forward * shootForce, ForceMode.Impulse);
        }
    }
}
