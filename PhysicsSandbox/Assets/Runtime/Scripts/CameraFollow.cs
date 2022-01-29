using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 arm;

    [SerializeField]
    private float followSmoothAcc = 20;

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.fixedDeltaTime * followSmoothAcc);
        transform.position = target.position + transform.rotation * arm;
    }
}
