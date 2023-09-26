using System;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    [SerializeField]private List<SensorBase> m_sensorBases;

    public void InitializeSensor(SensorName sensorName,Dictionary<SensorEventName,OnDetect> onDetects)
    {
        SensorBase sensorBase = m_sensorBases?.Find(x => x.SensorName == sensorName);

        if (sensorBase == null)
            throw new NullReferenceException($"Add {sensorName}");
        
        sensorBase.OnDetectEvents = onDetects;
    }
}
