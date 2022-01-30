using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 3;
    [SerializeField] private Color explosionCueColor = Color.red;

    [SerializeField] private ParticleSystem explosionParticles;
    private bool isCountingDown = false;

    private Renderer rend;

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

        explosionParticles.gameObject.SetActive(true);
        explosionParticles.transform.SetParent(null);
        explosionParticles.Play();

        var explosionFxTime = explosionParticles.main.duration;
        Destroy(gameObject, explosionFxTime);
        Destroy(explosionParticles.gameObject, explosionFxTime);

        //Disable ourselves (coroutine is going to end)
        gameObject.SetActive(false);
    }
}
