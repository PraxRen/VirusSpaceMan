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
}
