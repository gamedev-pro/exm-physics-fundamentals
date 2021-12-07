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
        float simulationTime = PhysicsUtils.EstimateTimeToReachYPosition(launchPoint.Position.y, endY, launchPoint.Velocity.y, launchPoint.Acceleration.y);

        var positionsCount = Mathf.RoundToInt(simulationTime / dt);
        var positions = new Vector2[positionsCount];
        if (positionsCount > 0)
        {
            positions[0] = launchPoint.Position;
            for (int i = 1; i < positions.Length; i++)
            {
                PhysicsUtils.SimulatePhysicsPoint(launchPoint, dt);
                positions[i] = launchPoint.Position;
            }
        }

        return positions;
    }

    public static float EstimateTimeToReachYPosition(float startY, float targetY, float vy, float ay)
    {
        float a = ay * 0.5f;
        float b = vy;
        float c = -(targetY - startY);

        float delta = b * b - 4 * a * c;
        if (delta < 0)
        {
            return 0;
        }

        float t1 = (-b + Mathf.Sqrt(delta)) / (2 * a);
        float t2 = (-b - Mathf.Sqrt(delta)) / (2 * a);
        //sempre vai ser t2 mas whatever
        return Mathf.Max(t1, t2);
    }

    public static void SimulatePhysicsPoint(PhysicsPoint point, float dt)
    {
        point.Velocity += point.Acceleration * dt;
        point.Position += point.Velocity * dt;
    }
}
