using System;
using System.Linq;
using MonoBehaviours.Behaviours;
using MonoBehaviours.Interface;
using MonoBehaviours.Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace Data
{
    [CreateAssetMenu(fileName = "GuardPatrolState", menuName = "AI/Guard/GuardPatrolState", order = 0)]
    public class GuardPatrolState :GuardBehaviorBase,IHasPathFinding
    {
        public Vector3[] SourceWaypoints;
            
        #region State
        
        public override void Initialize(GuardNPCMono guardNpc)
        {
            Vector3 des = guardNpc.PatrolWaypoints[0].position;
            SourceWaypoints = guardNpc.PatrolWaypoints.Select(x=>x.position).ToArray();
            
            if (m_PathfindingAlgorithmEnum == PathfindingAlgorithmEnum.NavMesh)
            {
                m_navMeshAgent = guardNpc.GetComponent<NavMeshAgent>();
                
                if (!m_navMeshAgent)
                    throw new NullReferenceException(
                        "NPC needs NavMesh Component when PathfindingAlgorithmEnum is set to NavMesh");
                
                m_navMeshAgent.SetDestination(guardNpc.PatrolWaypoints[0].position);
                guardNpc.SetWaypoints(m_navMeshAgent.path.corners.ToList());
                des = SourceWaypoints[0];
                InitializePathfinding(guardNpc.gameObject,m_navMeshAgent);
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
            NPC.SetCurrentWaypointInbox(0);
            NPC.ChangeAnimation(AnimatorParamName,AnimatorParamValue);

            if (m_PathfindingAlgorithmEnum == PathfindingAlgorithmEnum.NavMesh)
            {
                m_navMeshAgent.speed = Speed;
            }
        }

        public override void Tick()
        {
            if (NPC.PatrolWaypoints.Length > 0)
            {
                if (m_PathfindingAlgorithmEnum == PathfindingAlgorithmEnum.NavMesh)
                {
                    NavMeshAgent navMeshAgent = ((NavMeshPathfinder)_mPathfindingBase).NavMeshAgent;
                
                    if (Vector3.Distance(NPC.transform.position, NPC.CurrentTarget) <= navMeshAgent.stoppingDistance)
                    {
                        SetNextWayPoint();
                        navMeshAgent.path.ClearCorners();
                        navMeshAgent.SetDestination(NPC.CurrentTarget);
                    }
                    
                    return;
                }
                
                if (Vector3.Distance(NPC.transform.position,NPC.CurrentTarget) < .3f)
                {
                    SetNextWayPoint();
                }
            }
        }

        public void SetNextWayPoint()
        {
            NPC.SetCurrentWaypointInbox((NPC.CurrentWaypointInbox + 1) % NPC.PatrolWaypoints.Length);
            NPC.SetCurrentTarget(NPC.PatrolWaypoints[NPC.CurrentWaypointInbox].position);
            if (m_PathfindingAlgorithmEnum == PathfindingAlgorithmEnum.NavMesh)
            {
                ((NavMeshPathfinder)_mPathfindingBase).NavMeshAgent.path.ClearCorners();
                ((NavMeshPathfinder)_mPathfindingBase).NavMeshAgent.SetDestination(NPC.CurrentTarget);
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
            _mPathfindingBase = PathFindingManager.Instance.MakeInstanceOfPathFinding(npcGameObject,m_PathfindingAlgorithmEnum);
        }
        public void InitializePathfinding(GameObject npcGameObject,NavMeshAgent navMeshAgent)
        {
            _mPathfindingBase = PathFindingManager.Instance.MakeInstanceOfPathFinding(npcGameObject,navMeshAgent,m_PathfindingAlgorithmEnum);
        }
        
        #endregion
    }
}