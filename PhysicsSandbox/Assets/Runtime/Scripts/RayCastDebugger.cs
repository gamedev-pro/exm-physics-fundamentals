using UnityEngine;

public class RayCastDebugger : MonoBehaviour
{
    [SerializeField] private float rayDistance = 1;
    [SerializeField] private LayerMask collisionMask = ~0;

    private Vector3 RayOrigin => transform.position;
    private Vector3 RayDir => transform.forward;

    private float RayDistance => rayDistance;

    private void FixedUpdate()
    {
        Debug.DrawRay(RayOrigin, RayDir * RayDistance, Color.green);
        if (Physics.Raycast(RayOrigin, RayDir, out var hit, RayDistance, collisionMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.collider.name);
            Debug.DrawLine(transform.position, hit.point, Color.red);
            Debug.DrawRay(hit.point, hit.normal * 0.1f, Color.blue);
            Debug.DrawRay(transform.position + transform.up * 0.1f, RayDir * hit.distance, Color.cyan);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(RayOrigin, RayDir * RayDistance);
    }
}
