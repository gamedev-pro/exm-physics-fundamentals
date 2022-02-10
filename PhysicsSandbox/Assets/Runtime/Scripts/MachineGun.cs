using UnityEngine;
public class MachineGun : ROFWeapon
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private float rayDistance = 10;
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private float impactImpulse = 10;

    [Header("Visuals")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem impactFxPrefab;

    private Vector3 RayOrigin => muzzle.position;
    private Vector3 RayDir => muzzle.forward;

    protected override void Shoot()
    {
        Debug.DrawRay(RayOrigin, RayDir * rayDistance, Color.black);
        if (Physics.Raycast(
            RayOrigin,
            RayDir,
            out var hit,
            rayDistance,
            raycastMask,
            QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(RayOrigin, RayDir * rayDistance, Color.red, 1);
            if (hit.rigidbody != null)
            {
                var force = RayDir * impactImpulse;
                hit.rigidbody.AddForceAtPosition(force, hit.point, ForceMode.Impulse);
            }
            PlayHitFx(hit);
        }
    }

    private void PlayHitFx(in RaycastHit hit)
    {
        var fx = Instantiate(impactFxPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        //TODO (perf): MUST use object pool here
        Destroy(fx, fx.main.duration);
    }

    protected override void OnStartShoot()
    {
        base.OnStartShoot();
        muzzleFlash.gameObject.SetActive(true);
    }

    protected override void OnStopShoot()
    {
        base.OnStopShoot();
        muzzleFlash.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(RayOrigin, RayDir * rayDistance);
        }
    }
}
