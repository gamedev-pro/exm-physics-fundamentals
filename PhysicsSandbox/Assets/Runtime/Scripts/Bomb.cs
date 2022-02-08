using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 3;
    [SerializeField] private float explosionRadius = 2;
    [SerializeField] private float explosionForce = 100;

    [SerializeField] private float upwardsModifier = 0;
    [SerializeField] private LayerMask explosionMask;

    [SerializeField] private Color explosionCueColor = Color.red;

    [SerializeField] private ParticleSystem explosionParticles;
    private bool isCountingDown = false;

    private Renderer rend;

    private Collider[] collidersInRange = new Collider[20];

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        rend.material.EnableKeyword("_EMISSION");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isCountingDown)
        {
            isCountingDown = true;
            StartCoroutine(CountdownAndExplode());
        }
    }

    private IEnumerator CountdownAndExplode()
    {
        var explosionTime = Time.time + explosionDelay;
        while (Time.time < explosionTime)
        {
            var cuePercent = Mathf.PingPong(Time.time, 1);
            var cueColor = Color.Lerp(Color.black, explosionCueColor, cuePercent);
            rend.material.SetColor("_EmissionColor", cueColor);
            yield return null;
        }

        Explode();
    }

    private void Explode()
    {
        PlayExplosionEffects();

        //Overlap
        var collidersCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            explosionRadius,
            collidersInRange,
            explosionMask,
            QueryTriggerInteraction.Ignore
        );

        for (int i = 0; i < collidersCount; i++)
        {
            var collider = collidersInRange[i];
            if (collider.TryGetComponent<Rigidbody>(out var rb))
            {
                //linear decay (LERP)
                /* var toRb = rb.position - transform.position; */
                /* /1* Mathf.InverseLerp *1/ */
                /* var percent = Mathf.Clamp01((explosionRadius - toRb.magnitude) / explosionRadius); */
                /* var force = explosionForce * toRb.normalized * percent; */
                /* rb.AddForce(force, ForceMode.Impulse); */
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
            }
        }
    }

    private void PlayExplosionEffects()
    {
        explosionParticles.gameObject.SetActive(true);
        explosionParticles.transform.SetParent(null);
        explosionParticles.Play();

        var explosionFxTime = explosionParticles.main.duration;
        Destroy(gameObject, explosionFxTime);
        Destroy(explosionParticles.gameObject, explosionFxTime);

        //Disable ourselves (coroutine is going to end)
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
