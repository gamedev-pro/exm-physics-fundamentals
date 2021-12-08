using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CastOption
{
    Raycast, BoxCast
}
public class RaycastVisualizer : MonoBehaviour
{
    [SerializeField] private CastOption castOption;
    [SerializeField] private LayerMask mask;

    [SerializeField] private float rayLength;

    [SerializeField] private Vector2 rayDirection;

    [SerializeField] private Text collisionResultText;

    [Header("Box Cast")]
    [SerializeField]
    private Vector2 boxSize;

    [SerializeField]
    private float boxAngle;

    private void OnDrawGizmos()
    {
        switch (castOption)
        {
            case CastOption.Raycast:
                DrawRaycast();
                break;
            case CastOption.BoxCast:
                DrawBoxCast();
                break;
        }
    }

    private void DrawRaycast()
    {
        var hitResult = Physics2D.Raycast(transform.position, rayDirection, rayLength, mask);

        Gizmos.color = hitResult ? Color.red : Color.white;

        Gizmos.DrawRay(transform.position, rayDirection.normalized * rayLength);

        collisionResultText.text = "";
        if (hitResult)
        {
            collisionResultText.text = $"Collider: {hitResult.collider.name}\nPoint: {hitResult.point}\nDistance: {hitResult.distance}";
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(hitResult.point, hitResult.normal);
        }
    }

    private void DrawBoxCast()
    {
        var hitResult = Physics2D.BoxCast(transform.position, boxSize, boxAngle, rayDirection, rayLength, mask);

        var hitColor = hitResult ? Color.red : Color.gray;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0, 0, boxAngle), Vector3.one);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        var boxEndPosition = (Vector2)transform.position + rayDirection.normalized * rayLength;
        Gizmos.matrix = Matrix4x4.TRS(boxEndPosition, Quaternion.Euler(0, 0, boxAngle), Vector3.one);
        Gizmos.color = hitColor;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawRay(transform.position, rayDirection.normalized * rayLength);

        collisionResultText.text = "";
        if (hitResult)
        {
            collisionResultText.text = $"Collider: {hitResult.collider.name}\nPoint: {hitResult.point}\nDistance: {hitResult.distance}";
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(hitResult.point, hitResult.normal);
        }
    }
}
