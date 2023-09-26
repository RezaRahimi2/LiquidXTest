using System;
using System.Collections.Generic;
using MonoBehaviours.Behaviours;
using MonoBehaviours.Interface;
using MonoBehaviours.Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Data
{
    [CreateAssetMenu(fileName = "GuardOnLostPlayerState", menuName = "AI/Guard/GuardOnLostPlayerState", order = 0)]
    public class OnLostPlayerState : GuardBehaviorBase,IHasPathFinding
    {
        [SerializeField] private bool m_generateRandomNumber;
        [SerializeField] private Vector3 m_lastKnownPosition;
        private List<Vector3> m_points;
        private Vector3 m_currentPosition;
        public float radius = 5f;
        public int numberOfPoints = 10;
        
        #region State

        public override void Initialize(GuardNPCMono guardNpc)
        {
            if (m_PathfindingAlgorithmEnum == PathfindingAlgorithmEnum.NavMesh)
            {
                m_navMeshAgent = guardNpc.GetComponent<NavMeshAgent>();

                if (!m_navMeshAgent)
                    throw new NullReferenceException(
                        "NPC needs NavMesh Component when PathfindingAlgorithmEnum is set to NavMesh");
                
                InitializePathfinding(guardNpc.gameObject, m_navMeshAgent);
            }
            else
                InitializePathfinding(guardNpc.gameObject);

            guardNpc.SetPathFindingComponent(_mPathfindingBase);
            guardNpc.SetCurrentWaypointInbox(0);
            
            base.Initialize(guardNpc);
        }
        
        public override void OnEnter()
        {
            NPC.ChangeAnimation(AnimatorParamName,AnimatorParamValue);
            
            if (m_PathfindingAlgorithmEnum == PathfindingAlgorithmEnum.NavMesh)
            {
                m_navMeshAgent.speed = Speed;
                
                if (NPC.PlayerOnSight)
                    m_lastKnownPosition = NPC.LastPlayerSeenPosition;
                else
                    m_lastKnownPosition = NPC.LastHeardSoundPosition;
                
                m_navMeshAgent.SetDestination(m_lastKnownPosition);
            }
            
            m_points = GenerateRandomPoints(radius, numberOfPoints);
            m_points.Insert(0,m_lastKnownPosition);
            NPC.SetWaypoints(m_points);
            NPC.SetCurrentTarget(m_lastKnownPosition);
        }

        public override void Tick()
        {
            if (m_points.Count > 0 && Vector3.Distance(NPC.transform.position,NPC.CurrentTarget) < m_navMeshAgent.stoppingDistance)
            {
                m_currentPosition = m_points[^1];
                m_points.Remove(m_currentPosition);
                NPC.SetCurrentTarget(m_currentPosition);
                m_navMeshAgent.path.ClearCorners();
                m_navMeshAgent.SetDestination(m_currentPosition);
                if (m_points.Count == 0)
                    NPC.ChangeState(NPCStateNameEum.Patrolling);
            }
        }
        
        #endregion
        
        List<Vector3> GenerateRandomPoints(float radius, int numberOfPoints)
        {
            List<Vector3> randomPoints = new List<Vector3>();
            
            for (int i = 0; i < numberOfPoints; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                float sqrtRadius = Mathf.Sqrt(Random.value) * radius;
                float x = m_lastKnownPosition.x + sqrtRadius * Mathf.Cos(angle);
                float z = m_lastKnownPosition.z + sqrtRadius * Mathf.Sin(angle);
                randomPoints.Add(new Vector3(x, m_lastKnownPosition.y, z));
            }
            return randomPoints;
        }
        
        Vector3 FindNearestPoint(Vector3 currentPosition, List<Vector3> points)
        {
            Vector3 closestPoint = Vector3.zero;
            float closestDistanceSqr = Mathf.Infinity;

            foreach(Vector3 point in points)
            {
                Vector3 directionToTarget = point - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if(dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestPoint = point;
                }
            }

            return closestPoint;
        }

        #region Pathfinding

        [SerializeField] private PathfindingAlgorithmEnum m_PathfindingAlgorithmEnum;
        [SerializeField] private IPathfindingBase _mPathfindingBase;

        public IPathfindingBase PathfindingBase => _mPathfindingBase;
        public PathfindingAlgorithmEnum PathfindingAlgorithmEnum => m_PathfindingAlgorithmEnum;

        public void InitializePathfinding(GameObject npcGameObject)
        {
            _mPathfindingBase =
                PathFindingManager.Instance.MakeInstanceOfPathFinding(npcGameObject, m_PathfindingAlgorithmEnum);
        }

        public void InitializePathfinding(GameObject npcGameObject, NavMeshAgent navMeshAgent)
        {
            _mPathfindingBase =
                PathFindingManager.Instance.MakeInstanceOfPathFinding(npcGameObject, navMeshAgent,
                    m_PathfindingAlgorithmEnum);
        }

        #endregion
    }
}