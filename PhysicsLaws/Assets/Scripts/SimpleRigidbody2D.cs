using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

    [Min(0)]
    public float LinearDrag;

    public float InverseMass { get; private set; }

    private void Awake()
    {
        UpdateInverseMass();

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

    private void OnValidate()
    {
        UpdateInverseMass();
    }

    private void UpdateInverseMass()
    {
        Assert.IsFalse(Mathf.Approximately(mass, 0), "Massa de 0 não é suportada");
        InverseMass = Mathf.Approximately(mass, 0) ? 0 : 1.0f / mass;
    }
}
