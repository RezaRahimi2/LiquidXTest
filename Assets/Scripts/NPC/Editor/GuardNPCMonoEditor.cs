using MonoBehaviours.NPCs.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GuardNPCMono))]
public class GuardNPCMonoEditor : NpcEditor
{
    private Transform m_patrolWaypointsParent;
    private bool updateWP;
    public override void OnInspectorGUI()
    {
        // Fetch the serialized object
        serializedObject.Update();

        // Draw custom fields
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Fill the \"AI Properties\" and \"Fields\" fields",MessageType.Info);
        EditorGUILayout.LabelField("AI Properties", EditorStyles.boldLabel);
        EditorGUI.indentLevel = 1;
        SerializedProperty initializeStateNameEum = serializedObject.FindProperty("m_initializeStateNameEum");
        EditorGUILayout.PropertyField(initializeStateNameEum);
        SerializedProperty NpcStates = serializedObject.FindProperty("NpcStates");
        EditorGUILayout.PropertyField(NpcStates, true);

        EditorGUI.indentLevel = 0;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Fields", EditorStyles.boldLabel);
        EditorGUI.indentLevel = 1;
        SerializedProperty NavMeshAgent = serializedObject.FindProperty("m_navMeshAgent");
        EditorGUILayout.PropertyField(NavMeshAgent);
        SerializedProperty sensorController = serializedObject.FindProperty("m_sensorController");
        EditorGUILayout.PropertyField(sensorController);
        SerializedProperty dynamicWayPointsParent = serializedObject.FindProperty("m_dynamicWayPointsParent");
        EditorGUILayout.PropertyField(dynamicWayPointsParent);
        
        EditorGUI.indentLevel = 0;
        EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);
        EditorGUI.indentLevel = 1;
        SerializedProperty animationDatas = serializedObject.FindProperty("m_animationDatas");
        EditorGUILayout.PropertyField(animationDatas);
        SerializedProperty animationController = serializedObject.FindProperty("m_animationController");
        EditorGUILayout.PropertyField(animationController);
        
        EditorGUI.indentLevel = 0;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
        EditorGUI.indentLevel = 1;
        SerializedProperty currentStateName = serializedObject.FindProperty("CurrentStateName");
        EditorGUILayout.PropertyField(currentStateName);
        SerializedProperty currentTarget = serializedObject.FindProperty("m_currentTarget");
        EditorGUILayout.PropertyField(currentTarget);
        EditorGUILayout.LabelField("Player Data", EditorStyles.boldLabel);
        EditorGUI.indentLevel = 2;
        SerializedProperty playerOnSight = serializedObject.FindProperty("m_playerOnSight");
        EditorGUILayout.PropertyField(playerOnSight);
        SerializedProperty detectedPlayerGameObject = serializedObject.FindProperty("m_detectedPlayerGameObject");
        EditorGUILayout.PropertyField(detectedPlayerGameObject);
        SerializedProperty lastPlayerSeenPosition = serializedObject.FindProperty("m_lastPlayerSeenPosition");
        EditorGUILayout.PropertyField(lastPlayerSeenPosition);
        SerializedProperty LastSoundHeard = serializedObject.FindProperty("m_LastSoundHeard");
        EditorGUILayout.PropertyField(LastSoundHeard);
        EditorGUI.indentLevel = 1;
        EditorGUILayout.LabelField("WayPoints Data", EditorStyles.boldLabel);
        EditorGUI.indentLevel = 2;
        GameObject waypointsGO = GameObject.Find($"Waypoints/{target.name}/Static");
        if (!waypointsGO)
        {
            waypointsGO = GameObject.Find("Waypoints");
            if(!waypointsGO)
                waypointsGO = new GameObject("Waypoints");
            GameObject targetName = new GameObject(target.name);
           targetName.transform.SetParent(waypointsGO.transform);
           WayPointsController wayPointsController = targetName.AddComponent<WayPointsController>();
           GameObject staticWP = new GameObject("Static");
           staticWP.transform.SetParent(targetName.transform);
           wayPointsController.GuardNpcMono = target as GuardNPCMono;
           wayPointsController.StaticWayPointParent = staticWP.transform;
           wayPointsController.NpcName = target.name;
           GameObject dynamicWP = new GameObject("Dynamic");
           dynamicWP.transform.SetParent(targetName.transform);
           waypointsGO = staticWP;
           dynamicWayPointsParent.objectReferenceValue = dynamicWP;
           Selection.activeGameObject= targetName.gameObject;
           updateWP = true;
        }

        m_patrolWaypointsParent = EditorGUILayout.ObjectField("Patrol WayPoints Parent", waypointsGO.transform, typeof(Transform), true) as Transform;
        SerializedProperty patrolWaypoints = serializedObject.FindProperty("m_patrolWaypoints");
        EditorGUILayout.PropertyField(patrolWaypoints, true);
        SerializedProperty currentWaypoints = serializedObject.FindProperty("m_currentWaypoints");
        EditorGUILayout.PropertyField(currentWaypoints, true);
        SerializedProperty currentWaypointIndex = serializedObject.FindProperty("m_currentWaypointIndex");
        EditorGUILayout.PropertyField(currentWaypointIndex);
        EditorGUI.indentLevel = 0;
        base.OnInspectorGUI();

        if (GUILayout.Button("Refresh Waypoints") || updateWP)
        {
            if (m_patrolWaypointsParent != null && m_patrolWaypointsParent.childCount != patrolWaypoints.arraySize)
            {
                patrolWaypoints.arraySize = m_patrolWaypointsParent.childCount;

                for (int i = 0; i < patrolWaypoints.arraySize; i++)
                {
                    SerializedProperty patrolWaypoint = patrolWaypoints.GetArrayElementAtIndex(i);
                    patrolWaypoint.objectReferenceValue = m_patrolWaypointsParent.GetChild(i);
                }
            }
        }

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}
