using MonoBehaviours.Pathfinding;
using UnityEngine;

namespace MonoBehaviours.Interface
{
    public interface IHasPathFinding
    {
        public PathfindingAlgorithmEnum PathfindingAlgorithmEnum { get; }
        public IPathfindingBase PathfindingBase { get; }

        public void InitializePathfinding(GameObject sourceGameObject);
    }
}