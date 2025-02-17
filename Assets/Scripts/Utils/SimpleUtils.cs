using UnityEngine;

public class SimpleUtils
{
    public static bool IsLayerInclud(GameObject gameObject, LayerMask layerMask)
    {
        return IsLayerInclud(gameObject.layer, layerMask);
    }

    public static bool IsLayerInclud(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) > 0;
    }

    public static bool TryLuck(float value)
    {
        value = Mathf.Clamp01(value);
        return Random.value <= value;
    }

    public static float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        if (a.magnitude == 0 || b.magnitude == 0)
            return 0;

        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));
        float signed_angle = angle * sign;

        return signed_angle;
    }

    public static Vector3 GetRandomPositionInsideCircle(Vector3 center, float radius, float minRadius = 0f)
    {
        minRadius = Mathf.Clamp(minRadius, 0f, radius);
        float randomRadius = Mathf.Sqrt(Random.Range(minRadius * minRadius / (radius * radius), 1f)) * radius;
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Cos(angle) * randomRadius;
        float z = Mathf.Sin(angle) * randomRadius;

        return center + new Vector3(x, 0f, z);
    }

    public static Vector3 GetVectorDirection(Axis axis) 
    {
        return axis switch
        {
            Axis.X => Vector3.right,
            Axis.NegativeX => Vector3.left,
            Axis.Y => Vector3.up,
            Axis.NegativeY => Vector3.down,
            Axis.Z => Vector3.forward,
            Axis.NegativeZ => Vector3.back,
            _ => throw new System.NotImplementedException(),
        };
    }
}