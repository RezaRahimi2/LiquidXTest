using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviours.Pathfinding
{
    public interface IPathfindingBase
    {
        public abstract List<Vector3> FindPath(Vector3 start, Vector3 dest);
    }
    
    public abstract class PathfindingBase : IPathfindingBase
    {
        public GameObject GameObject { get; protected set; }
    
        public virtual void Initialize(GameObject npcGameObject, NavMeshAgent navMeshAgent)
        {
        }
        public abstract List<Vector3> FindPath(Vector3 start, Vector3 dest);
    }
}