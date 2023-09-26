using UnityEngine;

namespace MonoBehaviours.Interface
{
    public interface IHasPlayerTransform
    {
        public Transform PlayerTransform { get; set; }

        public void InitializePlayerTransform(Transform playerTransform);
    }
}