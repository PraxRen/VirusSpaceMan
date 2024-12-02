using UnityEngine;

public class SimpleUtils
{
    public static bool IsLayerInclud(GameObject gameObject, LayerMask layerMask)
    {
        return (layerMask.value & (1 << gameObject.layer)) > 0;
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
}