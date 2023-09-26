using UnityEngine;

[ExecuteInEditMode]
public class WayPointsDebugDrawer  : MonoBehaviour
{
    [SerializeField]private GuardNPCMono m_guardNpcMono;

    private void OnEnable()
    {
        m_guardNpcMono = GetComponent<GuardNPCMono>();
    }

    private void OnDrawGizmos()
    {
        if (m_guardNpcMono.PatrolWaypoints == null || m_guardNpcMono.PatrolWaypoints.Length == 0)
            return;
        
        if (m_guardNpcMono.CurrentStateName == NPCStateNameEum.Patrolling || !Application.isPlaying)
        {
            for (var i = m_guardNpcMono.PatrolWaypoints.Length - 1; i >= 0; i--)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(m_guardNpcMono.PatrolWaypoints[i].position, 0.5f);
                Gizmos.color = Color.yellow;
                if (i > 0)
                    Gizmos.DrawLine(m_guardNpcMono.PatrolWaypoints[i - 1].position, m_guardNpcMono.PatrolWaypoints[i].position);
                if (i == m_guardNpcMono.PatrolWaypoints.Length - 1)
                    Gizmos.DrawLine(m_guardNpcMono.PatrolWaypoints[i].position, m_guardNpcMono.PatrolWaypoints[0].position);
            }
        }
        else if(m_guardNpcMono.CurrentStateName == NPCStateNameEum.Lost)
        {
            for (var i = m_guardNpcMono.CurrentWaypoints.Length - 1; i >= 0; i--)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(m_guardNpcMono.CurrentWaypoints[i], 0.5f);
                Gizmos.color = Color.yellow;
                if (i > 0)
                    Gizmos.DrawLine(m_guardNpcMono.CurrentWaypoints[i - 1], m_guardNpcMono.CurrentWaypoints[i]);
                if (i == m_guardNpcMono.CurrentWaypoints.Length - 1)
                    Gizmos.DrawLine(m_guardNpcMono.CurrentWaypoints[i], m_guardNpcMono.CurrentWaypoints[0]);
            }
        }
    }
}
