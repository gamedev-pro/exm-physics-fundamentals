using UnityEngine;

[RequireComponent(typeof(SimpleRigidbody2D))]
public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followForceMultiplier = 5;

    [Range(0, 2)]
    [SerializeField] private float stopForceRatio = 0.3f;

    private float FollowForceMultiplier => followForceMultiplier * rb.Mass;
    private float StopForceMultiplier => FollowForceMultiplier * stopForceRatio;

    private SimpleRigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<SimpleRigidbody2D>();
    }

    private void FixedUpdate()
    {
        var toForce = CalculateFollowForce();
        var stopForce = CalculateStopForce();
        var netForce = toForce + stopForce;
        rb.AddForce(netForce, SimpleForceMode.Force);
    }

    private Vector2 CalculateFollowForce()
    {
        var toTarget = (Vector2)target.transform.position - rb.Position;
        return toTarget * FollowForceMultiplier;
    }

    private Vector2 CalculateStopForce()
    {
        return -rb.Velocity * rb.Mass * StopForceMultiplier;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            float forceDrawMultiplier = 0.5f / FollowForceMultiplier;
            const float drawThickness = 3.0f;
            var toForce = CalculateFollowForce() * forceDrawMultiplier;
            var stopForce = CalculateStopForce() * forceDrawMultiplier;
            var netForce = toForce + stopForce;

            Gizmos.color = Color.blue;
            GizmosUtils.DrawVector(transform.position, toForce, drawThickness);

            Gizmos.color = Color.yellow;
            GizmosUtils.DrawVector(transform.position, stopForce, drawThickness);

            Gizmos.color = Color.green;
            GizmosUtils.DrawVector(transform.position, netForce, drawThickness);
        }
    }
}
