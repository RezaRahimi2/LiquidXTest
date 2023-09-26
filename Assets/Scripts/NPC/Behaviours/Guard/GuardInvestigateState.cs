using System;
using System.Linq;
using MonoBehaviours.Behaviours;
using MonoBehaviours.Interface;
using MonoBehaviours.Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace Data
{
    [CreateAssetMenu(fileName = "GuardInvestigateState", menuName = "AI/Guard/GuardInvestigateState", order = 0)]
    public class GuardInvestigateState : GuardBehaviorBase, IHasPathFinding
    {
        private Vector3 m_lastKnownPosition;

        #region State

        public override void Initialize(GuardNPCMono guardNpc)
        {
            Vector3 des = guardNpc.PatrolWaypoints[0].position;
            if (m_PathfindingAlgorithmEnum == PathfindingAlgorithmEnum.NavMesh)
            {
                m_navMeshAgent = guardNpc.GetComponent<NavMeshAgent>();

                if (!m_navMeshAgent)
                    throw new NullReferenceException(
                        "GuardNpc needs NavMesh Component when PathfindingAlgorithmEnum is set to NavMesh");
                
                m_navMeshAgent.SetDestination(guardNpc.PatrolWaypoints[0].position);
                guardNpc.SetWaypoints(m_navMeshAgent.path.corners.ToList());
                des = guardNpc.CurrentWaypoints[0];
                InitializePathfinding(guardNpc.gameObject, m_navMeshAgent);
            }
            else
                InitializePathfinding(guardNpc.gameObject);

            guardNpc.SetPathFindingComponent(_mPathfindingBase);
            guardNpc.SetCurrentWaypointInbox(0);
            guardNpc.SetCurrentTarget(des);
            
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
            NPC.SetCurrentTarget(m_lastKnownPosition);
        }

        public override void Tick()
        {
            if (m_PathfindingAlgorithmEnum == PathfindingAlgorithmEnum.NavMesh)
            {
                if (Vector3.Distance(NPC.transform.position, m_lastKnownPosition) <= m_navMeshAgent.stoppingDistance)
                {
                    NPC.ChangeState(NPCStateNameEum.Lost);
                }
            }
        }
        
        #endregion

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

        public IPathfindingBase GetPathFinding(PathfindingAlgorithmEnum pathfindingAlgorithmEnum)
        {
            throw new NotImplementedException();
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