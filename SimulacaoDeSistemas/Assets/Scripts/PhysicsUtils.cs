using UnityEngine;

public class SimpleRigidBody2D
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;
}

public static class PhysicsUtils
{
    public static Vector2[] SimulateParabolicTrajectory(SimpleRigidBody2D launchPoint, float endY, float dt)
    {
        return new Vector2[0];
    }
}
