using System;
using MonoBehaviours.Behaviours;
using MonoBehaviours.Interface;
using MonoBehaviours.Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace Data
{
    [CreateAssetMenu(fileName = "GuardOnChasingState", menuName = "AI/Guard/GuardOnChasingState", order = 0)]
    public class GuardChasingState :GuardBehaviorBase,IHasPathFinding, IHasPlayerTransform
    {
        private Vector3 m_currentPosition;
        
        #region State
        
        public override void Initialize(GuardNPCMono guardNpc)
        {
            if (m_PathfindingAlgorithmEnum == PathfindingAlgorithmEnum.NavMesh)
            {
                m_navMeshAgent = guardNpc.GetComponent<NavMeshAgent>();

                if (!m_navMeshAgent)
                    throw new NullReferenceException(
                        "GuardNpc needs NavMesh Component when PathfindingAlgorithmEnum is set to NavMesh");
                
                InitializePathfinding(guardNpc.gameObject, m_navMeshAgent);
            }
            else
                InitializePathfinding(guardNpc.gameObject);

            guardNpc.SetPathFindingComponent(_mPathfindingBase);
            
            base.Initialize(guardNpc);

        }
        
        public override void OnEnter()
        {
            NPC.ChangeAnimation(AnimatorParamName,AnimatorParamValue);

            if (m_PathfindingAlgorithmEnum == PathfindingAlgorithmEnum.NavMesh)
            {
                m_navMeshAgent.speed = Speed;
            }
            NPC.SetCurrentTarget(NPC.DetectedPlayerGameObject.transform.position);
            InitializePlayerTransform(NPC.DetectedPlayerGameObject.transform);
        }

        public override void Tick()
        {
            m_navMeshAgent.SetDestination(PlayerTransform.transform.position);
            if (Vector3.Distance(NPC.transform.position, PlayerTransform.transform.position) < 2)
            {
                Debug.Log("Catch Player");
                NPC.ChangeAnimation(AnimatorParamName,0f);
            }
            else
            {
                NPC.ChangeAnimation(AnimatorParamName,AnimatorParamValue);
            }
        }
        
        #endregion

        public Transform PlayerTransform { get; set; }

        public void InitializePlayerTransform(Transform playerTransform)
        {
            PlayerTransform = playerTransform;
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