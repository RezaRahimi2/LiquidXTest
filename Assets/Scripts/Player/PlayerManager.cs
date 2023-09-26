using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player m_player;
    public Player Player => m_player;
}