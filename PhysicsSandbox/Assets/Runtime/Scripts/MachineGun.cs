using UnityEngine;

public class MachineGun : MonoBehaviour
{
    [SerializeField] private float rof = 2;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float rayDistance = 10;
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private float impactImpulse = 10;

    [Header("Visuals")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem impactFxPrefab;

    private Vector3 RayOrigin => muzzle.position;
    private Vector3 RayDir => muzzle.forward;
    private float ShootDelay => 1.0f / rof;
    private float nextShootTime;

    private bool wantsToShoot = false;

    private void Awake()
    {
        StopShoot();
    }

    private void FixedUpdate()
    {
        if (wantsToShoot)
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (Time.time > nextShootTime)
        {
            nextShootTime = Time.time + ShootDelay;

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
    }

    public void StartShoot()
    {
        wantsToShoot = true;
        enabled = true;
        muzzleFlash.gameObject.SetActive(true);
    }

    public void StopShoot()
    {
        wantsToShoot = false;
        enabled = false;
        muzzleFlash.gameObject.SetActive(false);
    }


    private void PlayHitFx(in RaycastHit hit)
    {
        var fx = Instantiate(impactFxPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        //TODO (perf): MUST use object pool here
        Destroy(fx, fx.main.duration);
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
