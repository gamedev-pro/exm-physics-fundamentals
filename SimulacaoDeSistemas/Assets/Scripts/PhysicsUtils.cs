using UnityEngine;

public class PhysicsPoint
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;
}

public static class PhysicsUtils
{
    public static Vector2[] SimulateParabolicTrajectory(PhysicsPoint launchPoint, float endY, float dt)
    {
        return new Vector2[0];
    }
}
