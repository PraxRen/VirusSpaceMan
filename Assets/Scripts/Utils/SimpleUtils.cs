using UnityEngine;

public class SimpleUtils
{
    public static bool IsLayerInclud(GameObject gameObject, LayerMask layerMask)
    {
        return (layerMask.value & (1 << gameObject.layer)) > 0;
    }

    public static bool TryLuck(float maxValue, float value)
    {
        return Random.Range(0, maxValue) <= value;
    }
}
