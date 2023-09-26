using UnityEngine;

namespace MonoBehaviours
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_instance;

        public static GameManager Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = FindObjectOfType<GameManager>();
                return m_instance;
            }
        }

        [field: SerializeField]
        public PlayerManager PlayerManager { get; private set; }
    }
}