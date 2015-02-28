using UnityEngine;

public static class VectorExtensions
{
    public static float Angle(this Vector2 v1, Vector2 v2)
    {
        return Mathf.Atan2(v1.x * v2.y - v1.y * v2.x, v1.x * v2.x + v1.y * v2.y);
    }

    public static float Angle2(this Vector2 v, Vector2 vLeft, Vector2 vRight)
    {
        return (vLeft - v).Angle(vRight - v);
    }

    public static Vector2 Rotate(this Vector2 v, Vector2 origin, float amount)
    {
        float x = v.x - origin.x;
        float y = v.y - origin.y;

        return new Vector2(
            x * Mathf.Cos(amount) - y * Mathf.Sin(amount) + origin.x,
            x * Mathf.Sin(amount) - y * Mathf.Cos(amount) + origin.y);
    }

    public static float Length2(this Vector2 v)
    {
        return v.x * v.x + v.y * v.y;
    }

    public static float Length(this Vector2 v)
    {
        return Mathf.Sqrt(v.x * v.x + v.y * v.y);
    }

    public static float Length(this Vector3 v)
    {
        return Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
    }

    public static Vector3 Divide(this Vector3 v, Vector3 divisor)
    {
        return new Vector3(v.x / divisor.x, v.y / divisor.y, v.z / divisor.z);
    }
}
