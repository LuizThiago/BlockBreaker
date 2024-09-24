using UnityEngine;

public static class Utils
{
    public static bool ContainsLayerMask(LayerMask layerMask, GameObject obj)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }
}
