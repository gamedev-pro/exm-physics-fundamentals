using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 3;
    [SerializeField] private float explosionRadius = 2;
    [SerializeField] private float explosionForce = 100;
    [Range(0, 5)]
    [SerializeField] private float upwardsModifider = 2;
    [SerializeField] private LayerMask explosionMask = ~0;
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

        // Em geral queremos sempre usar NonAlloc
        /* var collidersInRange = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, collidersInRange, explosionMask, QueryTriggerInteraction.Ignore); */
        var overlapCount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, collidersInRange, explosionMask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < overlapCount; i++)
        {
            var collider = collidersInRange[i];
            if (collidersInRange[i].TryGetComponent<Rigidbody>(out var rb))
            {
                // Aplicando forca manualmente com linear decay
                /* var toRb = rb.position - transform.position; */
                /* var percent = (explosionRadius - toRb.magnitude) / explosionRadius; */
                /* var force = explosionForce * percent; */
                /* rb.AddForce(toRb.normalized * force, ForceMode.Impulse); */

                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifider, ForceMode.Impulse);
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
