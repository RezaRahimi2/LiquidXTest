using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonoBehaviours.Utility.Editor
{
    [CustomEditor(typeof(WayPointsController))]
    public class WaypointsControllerEditor : UnityEditor.Editor
    {
        private WayPointsController m_waypointsController;
        public override VisualElement CreateInspectorGUI()
        {
            m_waypointsController = target as WayPointsController;
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add a new Waypoint"))
            {
                GameObject newWp = new GameObject($"{m_waypointsController.NpcName}_WP{m_waypointsController.StaticWayPointParent.childCount}");
                newWp.transform.SetParent(m_waypointsController.StaticWayPointParent);
                m_waypointsController.UpdateWps();
                AssetDatabase.Refresh();
            }
        }
    }
}