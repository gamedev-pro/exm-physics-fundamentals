using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCastDebugger : MonoBehaviour
{
    [SerializeField] private Vector3 boxExtents = Vector3.one;
    [SerializeField] private float rayDistance = 1;
    [SerializeField] private LayerMask collisionMask = ~0;

    private Vector3 RayOrigin => transform.position;
    private Vector3 RayDir => transform.forward;

    private float RayDistance => rayDistance;

    private void FixedUpdate()
    {
        Debug.DrawRay(RayOrigin, RayDir * RayDistance, Color.green);
        if (Physics.BoxCast(RayOrigin, boxExtents, RayDir, out var hit, transform.rotation, RayDistance, collisionMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.collider.name);
            Debug.DrawLine(transform.position, hit.point, Color.red);
            Debug.DrawRay(hit.point, hit.normal * 0.1f, Color.blue);
            Debug.DrawRay(transform.position + RayDir * boxExtents.x + transform.up * 0.1f, RayDir * hit.distance, Color.cyan);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(RayOrigin, RayDir * RayDistance);

        Gizmos.matrix = Matrix4x4.TRS(RayOrigin, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxExtents * 2);

        Gizmos.color = Color.grey;
        Gizmos.matrix = Matrix4x4.TRS(RayOrigin + RayDir * RayDistance, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxExtents * 2);

        Gizmos.matrix = Matrix4x4.identity;
    }
}

