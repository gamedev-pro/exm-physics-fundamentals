using UnityEngine;

public static class DebugUtils
{
    public static void DrawBox(Vector2 center, Vector2 size, Color color, float duration = 0)
    {
        var bottomLeft = center - size;
        var topLeft = center + new Vector2(-size.x, size.y);
        var bottomRight = center + new Vector2(size.x, -size.y);
        var topRight = center + size;

        Debug.DrawLine(bottomLeft, topLeft, color, duration);
        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
    }
}