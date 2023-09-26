using System;
using System.Collections.Generic;
using Events;
using MonoBehaviours.Interface;
using MonoBehaviours.Pathfinding;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StateMachine))]
public class GuardNPCMono : NPC<GuardNPCMono>,ICanSee,IHasAnimation
{
    #region Fields
    // PatrolWaypoints are the points that the NPC patrols between.
    [SerializeField] private Transform[] m_patrolWaypoints;
    public Transform[] PatrolWaypoints => m_patrolWaypoints;

    // NavMeshAgent is a component that can move the NPC along the navigation mesh in the game.
    [SerializeField] private NavMeshAgent m_navMeshAgent;
    public NavMeshAgent NavMeshAgent
    {
        get => m_navMeshAgent;
        private set => m_navMeshAgent = value;
    }

    // SensorController is used to detect players or other objects in the game.
    [SerializeField]private SensorController m_sensorController;
    
    // CurrentTarget represents the current target position that the NPC is moving towards.
    [SerializeField]private Vector3 m_currentTarget;
    public Vector3 CurrentTarget => m_currentTarget;
    
    // LastPlayerSeenPosition represents the last position where the NPC saw the player.
    [SerializeField] private Vector3 m_lastPlayerSeenPosition;
    public Vector3 LastPlayerSeenPosition => m_lastPlayerSeenPosition;
    
    // DetectedPlayerGameObject represents the player object that the NPC has detected.
    [SerializeField] private GameObject m_detectedPlayerGameObject;
    public GameObject DetectedPlayerGameObject => m_detectedPlayerGameObject;

    // CurrentWaypoints are the points that the NPC is currently moving between.
    [SerializeField]private Vector3[] m_currentWaypoints;
    public Vector3[] CurrentWaypoints => m_currentWaypoints;
    
    //Store the current waypoint index
    [SerializeField] private int m_currentWaypointIndex = 0;
    public int CurrentWaypointInbox => m_currentWaypointIndex;
    
    // PlayerOnSight indicates whether the NPC can currently see the player.
    [SerializeField] private bool m_playerOnSight;
    public bool PlayerOnSight => m_playerOnSight;
    
    // LastSoundHeard represents the last position where the NPC heard a sound.
    [SerializeField]private Vector3 m_LastSoundHeard;
    public Vector3 LastHeardSoundPosition => m_LastSoundHeard; 

    // PathfindingBase is used to calculate the path for the NPC to move along.
    private IPathfindingBase m_pathfindingBase;

    // DynamicWayPointsParent is the parent transform for the dynamic waypoints.
    [SerializeField]private Transform m_dynamicWayPointsParent;

    #endregion

    #region MonoBehaviour

     private void Start()
    {
        AnimationController.Initialize(AnimationDatas);
        
        // Initialize each state in the NPC's state list.
        NpcStates.ForEach(x =>
        {
            x.Initialize(this);
            IHasAnimationData hasAnimation = (x as IHasAnimationData);

            if (hasAnimation != null && string.IsNullOrEmpty(hasAnimation.AnimatorParamName))
            {
                throw new NullReferenceException($"Select AnimatorParamName is {x.name} ScriptableObject");
            }
            
        });
        
        // Create a new dictionary to hold the different sensor events and their corresponding methods.
        Dictionary<SensorEventName, OnDetect> onDetects = new Dictionary<SensorEventName, OnDetect>();
        
        //Chasing the player after in Near FOV.
        onDetects.Add(SensorEventName.OnNearDetect, OnDetectPlayerInNearFOV);
        
        //Investigating the player after in Far FOV.
        onDetects.Add(SensorEventName.OnFarDetect, OnDetectPlayerInFarFOV);
        
        //Run Lost state after NPC lost the player(can't see or hear the player)
        onDetects.Add(SensorEventName.OnLostPlayer, OnLostPlayer);
        
        // Initialize the Field of View (FOV) sensor with the sensor events.
        m_sensorController.InitializeSensor(SensorName.FovSensor,onDetects);

        // Create a new dictionary for the sound sensor events.
        onDetects = new Dictionary<SensorEventName, OnDetect>(); 
        
        // Add the OnSoundDetect event and its corresponding method to the dictionary.
        onDetects.Add(SensorEventName.OnSoundDetect, OnDetectPlayerBySound);
        
        // Initialize the Sound sensor with the sensor events.
        m_sensorController.InitializeSensor(SensorName.SoundSensor,onDetects);
        
        // Get the StateMachine component attached to this GameObject.
        StateMachine = GetComponent<StateMachine>();
        
        // Initialize the StateMachine with the initial state.
        StateMachine.Initialize(GetStateByName(m_initializeStateNameEum));
    }
    
    #endregion

    #region Methods

    public void SetPathFindingComponent(IPathfindingBase pathfinding)
    {
        m_pathfindingBase = pathfinding;
    }

    public void SetCurrentTarget(Vector3 _currentTransform)
    {
        m_currentTarget = _currentTransform;
    }

    public void SetWaypoints(List<Vector3> _waypoints)
    {
        if(_waypoints.Count == 0)
            return;

        if (m_dynamicWayPointsParent.childCount > 0)
        {
            for (int i = 0; i < m_dynamicWayPointsParent.childCount; i++)
            {
                Destroy(m_dynamicWayPointsParent.GetChild(i).gameObject);
            }
        }
        
        for (var i = _waypoints.Count - 1; i >= 0; i--)
        {
            GameObject newWayPointGO = new GameObject($"WP{i}");
            newWayPointGO.transform.SetParent(m_dynamicWayPointsParent);
            newWayPointGO.transform.position = new Vector3(_waypoints[i].x,_waypoints[i].y,_waypoints[i].z);
        }

        m_currentWaypoints = _waypoints.ToArray();
        m_currentWaypointIndex = 0;
    }

    public void SetCurrentWaypointInbox(int index)
    {
        m_currentWaypointIndex = index;
    }

    #endregion

    #region Events

    public void OnDetectPlayerBySound(DetectData playerHeardPos)
    {
        m_LastSoundHeard = playerHeardPos.VectorData.Value;

        if(PlayerOnSight)
            return;
        
        ChangeState(NPCStateNameEum.Investigating);
    }
    
    public void OnDetectPlayerInFarFOV(DetectData playerSeenPos)
    {
        m_playerOnSight = true;
        m_lastPlayerSeenPosition = playerSeenPos.VectorData.Value;
        ChangeState(NPCStateNameEum.Investigating);
    }
    
    public void OnDetectPlayerInNearFOV(DetectData detectedGameObject)
    {
        if(CurrentStateName == NPCStateNameEum.Chasing)
            return;
        
        m_playerOnSight = true;
        m_detectedPlayerGameObject = detectedGameObject.GameObjectData; 
        ChangeState(NPCStateNameEum.Chasing);
    }

    public void OnLostPlayer(DetectData playerKnownPos)
    {
        m_playerOnSight = false;
        m_lastPlayerSeenPosition = playerKnownPos.VectorData.Value;
        ChangeState(NPCStateNameEum.Lost);
    }


    #endregion

    #region IHasAnimation

    [SerializeField] private List<AnimatorParamData> m_animationDatas;
    [SerializeField] private AnimationController m_animationController;

    public List<AnimatorParamData> AnimationDatas
    {
        get => m_animationDatas;
        set => m_animationDatas = value;
    }

    
    public AnimationController AnimationController
    {
        get => m_animationController;
        set => m_animationController = value;
    }

    public void ChangeAnimation(string animatorName,object value)
    {
        AnimationController.ChangeAnimation(animatorName,value);
    }

    public void InitializeAnimationController()
    {
        if (AnimationController == null)
            AnimationController = GetComponent<AnimationController>();
    }

    #endregion
    
#if UNITY_EDITOR
    public void UpdateWPs(Transform patrolWP)
    {
        m_patrolWaypoints = new Transform[patrolWP.childCount];
        for (int i = 0; i < patrolWP.childCount; i++)
        {
            m_patrolWaypoints[i] = patrolWP.GetChild(i);
        }
    }
#endif
}