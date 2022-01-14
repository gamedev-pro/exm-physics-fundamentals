using UnityEngine;

public class ParabolicMovementParameters
{
    public Vector2 Position;
    public Vector2 Velocity;

    public float Gravity;
}

public static class ParabolicMovementCalculator
{
    public static Vector2[] SimulateParabolicTrajectory(ParabolicMovementParameters launchParameters, float endY, float dt)
    {
        return new Vector2[0];
    }
}
