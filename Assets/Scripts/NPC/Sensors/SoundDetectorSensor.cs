using MonoBehaviours.Interface;

public sealed class SoundDetectorSensor : SensorBase, ICanHear
{ 
    protected override void Logic()
    {
        
    }

    public void OnDetectPlayerBySound(DetectData detectData)
    {
        if(!IsActive)
            return;
        
        OnDetectEvents[SensorEventName.OnSoundDetect].Invoke(detectData);
    }
}
