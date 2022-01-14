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
        float simulationTime = ParabolicMovementCalculator.EstimateTimeToReachYPosition(launchParameters, endY);

        var positionsCount = Mathf.RoundToInt(simulationTime / dt);
        var positions = new Vector2[positionsCount];
        var t = 0.0f;
        if (positionsCount > 0)
        {
            positions[0] = launchParameters.Position;
            for (int i = 1; i < positions.Length; i++)
            {
                t += dt;
                var posX = launchParameters.Position.x + launchParameters.Velocity.x * t;
                var posY = launchParameters.Position.y + launchParameters.Velocity.y * t - launchParameters.Gravity * t * t * 0.5f;
                positions[i] = new Vector2(posX, posY);
            }
        }

        return positions;
    }

    public static float EstimateTimeToReachYPosition(ParabolicMovementParameters initialParameters, float endY)
    {
        float a = -initialParameters.Gravity * 0.5f;
        float b = initialParameters.Velocity.y;
        float c = -(endY - initialParameters.Position.y);

        float delta = b * b - 4 * a * c;
        if (delta < 0)
        {
            return 0;
        }

        //sempre vai ser t2
        /* float t1 = (-b + Mathf.Sqrt(delta)) / (2 * a); */
        float t2 = (-b - Mathf.Sqrt(delta)) / (2 * a);
        return t2;
    }
}
