using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRigidbody2D : MonoBehaviour
{
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    public Vector2 Velocity;

    [SerializeField] private float mass = 1;

    public Vector2 Force;

    public float InverseMass => Mathf.Approximately(mass, 0.0f) ? 1.0f : 1.0f / mass;

    private void Awake()
    {
        //TODO: PhysicsWorld2D deve ser um singleton
        var physicsWorld = FindObjectOfType<PhysicsWorld2D>();
        physicsWorld.Register(this);
    }

    private void OnDestroy()
    {
        //TODO: PhysicsWorld2D deve ser um singleton
        var physicsWorld = FindObjectOfType<PhysicsWorld2D>();
        if (physicsWorld != null)
        {
            physicsWorld.Unregister(this);
        }
    }
}
