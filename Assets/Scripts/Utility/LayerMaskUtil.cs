using UnityEngine;

public class LayerMaskUtility
{
    public static bool CheckLayer(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }
}
