using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DefaultExecutionOrder(10000)]
public class SimpleBoxCollisionDetection : MonoBehaviour
{
    [SerializeField] private Vector3 boxExtents = Vector3.one * 0.1f;
    [SerializeField] private LayerMask collisionMask = ~0;

    [SerializeField]
    [Range(0.05f, 0.2f)]
    private float lookAheadDist = 0.05f;

    private RaycastHit[] hits = new RaycastHit[20];

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var frameDelta = rb.velocity * Time.fixedDeltaTime;
        var raycastParams = new RaycastParams
        {
            Origin = rb.position,
            Distance = frameDelta.magnitude + lookAheadDist,
            Direction = frameDelta.normalized,
        };

        // Collision Detection
        if (Physics.BoxCast(raycastParams.Origin, boxExtents, raycastParams.Direction, out var hit, transform.rotation, raycastParams.Distance, collisionMask))
        {
            //Collision Resolution
            ResolveCollision(hit, raycastParams, rb);
        }
    }

    private void ResolveCollision(in RaycastHit hit, in RaycastParams raycastParams, Rigidbody myRb)
    {
        // Collision Resolution
        var penetration = raycastParams.Distance - hit.distance;
        if (hit.rigidbody != null)
        {
            myRb.position -= raycastParams.Direction * penetration * 0.5f;
            hit.rigidbody.position += raycastParams.Direction * penetration * 0.5f;
        }
        else
        {
            myRb.position -= raycastParams.Direction * penetration;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxExtents * 2);
    }
}
