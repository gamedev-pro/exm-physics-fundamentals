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

    public Vector2 Acceleration;

    private void Awake()
    {
        //TODO: PhysicsWorld2D deve ser um singleton
        var physicsWorld = FindObjectOfType<PhysicsWorld2D>();
        physicsWorld.Register(this);
    }
}
