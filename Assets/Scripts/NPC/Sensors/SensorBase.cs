using System.Collections.Generic;
using System.Threading.Tasks;
using Events;
using UnityEngine;

public abstract class SensorBase : MonoBehaviour
{
    [SerializeField]protected bool IsActive;
    public SensorName SensorName;
    public Dictionary<SensorEventName,OnDetect> OnDetectEvents;
    
    [SerializeField]protected int UpdateRate;
    [SerializeField]protected bool PlayerIsInRange;
    [SerializeField]protected Vector3 LastPlayerDetectPosition;
    [SerializeField]protected LayerMask DetectionLayerMask;

    private void Start()
    {
        CustomUpdate();
    }

    protected async void CustomUpdate()
    {
        while (IsActive)
        {
            Logic();
            await Task.Delay(UpdateRate);   
        }
    }
    
    protected abstract void Logic();
}
