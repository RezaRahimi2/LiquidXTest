using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviours.Pathfinding
{
    public class PathFindingManager : Singleton<PathFindingManager>
    {
        public List<IPathfindingBase> PathfindingBaseMonos = new List<IPathfindingBase>();

        public CustomPathFindingBase MakeInstanceOfPathFinding(GameObject npcGameObject,
            PathfindingAlgorithmEnum pathfindingAlgorithmEnum)
        {
            CustomPathFindingBase pathfindingBase = null;
            switch (pathfindingAlgorithmEnum)
            {
                case PathfindingAlgorithmEnum.DFS:
                {
                    pathfindingBase = new DfsPathfinding();
                    break;
                }
                case PathfindingAlgorithmEnum.BFS:
                {
                    pathfindingBase = new BfsPathfinding();
                    break;
                }
                case PathfindingAlgorithmEnum.AStar:
                    break;
                default:
                    throw new NotImplementedException();
            }

            npcGameObject.AddComponent<NPCMovement>();
            PathfindingBaseMonos.Add(pathfindingBase);
            return pathfindingBase;
        }

        public IPathfindingBase MakeInstanceOfPathFinding(GameObject npcGameObject,NavMeshAgent navMeshAgent,
            PathfindingAlgorithmEnum pathfindingAlgorithmEnum)
        {
            NavMeshPathfinder pathfindingBase = new NavMeshPathfinder();
            pathfindingBase.Initialize(npcGameObject,navMeshAgent);
            return pathfindingBase;
        }
    }
}