namespace MonoBehaviours.Interface
{
    public interface ICanSee
    {
        public void OnDetectPlayerInFarFOV(DetectData playerSeenPos);

        public void OnDetectPlayerInNearFOV(DetectData detectedGameObject);

        public void OnLostPlayer(DetectData playerKnownPos);
    }
}