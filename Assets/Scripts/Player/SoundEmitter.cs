using MonoBehaviours.Interface;
using UnityEngine;

namespace MonoBehaviours.Player
{
    [ExecuteInEditMode]
    public class SoundEmitter : MonoBehaviour
    {
        [SerializeField] private LayerMask m_obstacleLayerMask;
        [SerializeField] private LayerMask m_layerMask;
        [SerializeField] private float m_lowSoundRadius;
        [SerializeField] private float m_highSoundRadius;
        [SerializeField] private float currentRadius; 
        public void Initialize(Character character)
        {
            character.OnMovementEvent += (speed,maxValue) => EmitSound(Mathf.Clamp(speed, 0, maxValue));
        }

        public void EmitSound(float speed)
        {
            currentRadius = speed;
            if (speed > 0)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentRadius, m_layerMask);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.TryGetComponent(out ICanHear sensor))
                    {
                        if (Physics.Linecast(transform.position, hitCollider.transform.position, out RaycastHit hit,
                                m_obstacleLayerMask))
                        {
                            // If the linecast hits an obstacle, the sound is blocked
                            continue;
                        }
                        else
                        {
                            sensor.OnDetectPlayerBySound(new DetectData(transform.position));
                        }
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position,m_lowSoundRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,m_highSoundRadius);
        }

        private void OnDrawGizmos()
        {
            if (currentRadius > 0)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position,currentRadius);
            }
        }
    }
}