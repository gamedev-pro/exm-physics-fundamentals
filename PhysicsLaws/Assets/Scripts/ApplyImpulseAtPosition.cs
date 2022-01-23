using UnityEngine;

[RequireComponent(typeof(SimpleRigidbody2D))]
public class ApplyImpulseAtPosition : MonoBehaviour
{
    [SerializeField] private Vector2 force;
    [SerializeField] private Vector2 relativePos;

    private Vector2 WorldPos => transform.TransformPoint(relativePos);

    private void Start()
    {
        GetComponent<SimpleRigidbody2D>().AddForceAtPosition(force, WorldPos, SimpleForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(WorldPos, 0.1f);
            GizmosUtils.DrawVector(WorldPos - force, force, 5);
        }
    }
}
