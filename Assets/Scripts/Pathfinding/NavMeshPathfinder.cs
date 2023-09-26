using System.Collections.Generic;
using System.Linq;
using MonoBehaviours.Pathfinding;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshPathfinder : PathfindingBase
{
    [SerializeField] private NavMeshAgent m_navMeshAgent;
    public NavMeshAgent NavMeshAgent => m_navMeshAgent;

    public override void Initialize(GameObject transform, NavMeshAgent navMeshAgent)
    {
        GameObject = transform;
        m_navMeshAgent = navMeshAgent;
    }

    public override List<Vector3> FindPath(Vector3 start, Vector3 dest)
    {
        m_navMeshAgent.SetDestination(dest);
        return m_navMeshAgent.path.corners.ToList();
    }
}
