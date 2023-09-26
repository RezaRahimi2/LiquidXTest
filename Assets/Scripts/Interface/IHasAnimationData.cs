namespace MonoBehaviours.Interface
{
    public interface IHasAnimationData
    {
        public string AnimatorParamName { get;}
        public float AnimatorParamValue { get;set; }
        public float  Speed { get;set; }
    }
}