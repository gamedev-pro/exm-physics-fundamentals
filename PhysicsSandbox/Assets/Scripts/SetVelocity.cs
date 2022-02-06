using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVelocity : MonoBehaviour
{
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 angularVelocity;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var rb = GetComponent<Rigidbody>();
            rb.velocity = velocity;
            rb.maxAngularVelocity = Mathf.Infinity;
            rb.angularVelocity = angularVelocity;
            enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, velocity);
    }
}
