using UnityEngine;

public class DetectData
{
    public Vector3? VectorData { get; set; }
    public GameObject GameObjectData { get; set; }

    public DetectData()
    {
        
    }
    public DetectData(Vector3 vector3)
    {
        VectorData = vector3;
    }
    
    public DetectData(GameObject gameObject)
    {
        GameObjectData = gameObject;
    }
}
