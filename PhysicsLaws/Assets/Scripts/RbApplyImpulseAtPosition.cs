using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RbApplyImpulseAtPosition : MonoBehaviour
{
    [SerializeField] private Vector2 force;
    [SerializeField] private Vector2 relativePos;

    private Vector2 WorldPos => transform.TransformPoint(relativePos);

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForceAtPosition(force, WorldPos, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(WorldPos, 0.1f);
            GizmosUtils.DrawVector(WorldPos - force, force, 5);
        }
    }
}
