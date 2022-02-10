using UnityEngine;

public abstract class ROFWeapon : MonoBehaviour
{
    [SerializeField] private float rof = 2;
    private float ShootDelay => 1.0f / rof;
    private float nextShootTime;

    private bool wantsToShoot = false;

    private void Awake()
    {
        StopShoot();
    }

    private void Update()
    {
        if (wantsToShoot && CanShoot())
        {
            TryShoot();
        }
    }

    public void StartShoot()
    {
        wantsToShoot = true;
        enabled = true;
        OnStartShoot();
    }

    public void StopShoot()
    {
        wantsToShoot = false;
        enabled = false;
        OnStopShoot();
    }

    private void TryShoot()
    {
        if (CanShoot())
        {
            nextShootTime = Time.time + ShootDelay;
            Shoot();
        }
    }

    //TODO: We could make this virtual
    public bool CanShoot()
    {
        return Time.time > nextShootTime;
    }

    protected abstract void Shoot();
    protected virtual void OnStartShoot()
    {

    }
    protected virtual void OnStopShoot()
    {

    }
}
