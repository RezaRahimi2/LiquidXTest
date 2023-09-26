using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoBehaviours.Behaviours;
using MonoBehaviours.Interface;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[CustomEditor(typeof(NpcBehaviorBase<>),true)]
public class NpcBehaviorBaseEditor : UnityEditor.Editor
{
    private Object m_targetObject;
    private Animator m_animator;
    private AnimationController m_animationController;
    private Character m_npc;
    private string[] m_parameters;
    public int m_index = 0;

    public override VisualElement CreateInspectorGUI()
    {
        m_animator = FindPrefabsWithObjectReference(m_targetObject,ref m_animationController);

       if(m_animator != null)
           m_parameters = ((AnimatorController)m_animator.runtimeAnimatorController).parameters.Select(x => x.name)
               .ToArray();
       
        return base.CreateInspectorGUI();
    }

    public override void OnInspectorGUI()
    {
        if (m_parameters != null && m_parameters?.Length > 0)
        {
            EditorGUILayout.LabelField("Animator Parameter Name:");
            m_index = EditorGUILayout.Popup(m_index, m_parameters);

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }

        base.OnInspectorGUI();

        if (GUILayout.Button("Refresh"))
        {
            m_animator = FindPrefabsWithObjectReference(target, ref m_animationController);
            // THIS IS THE SOLUTION
            m_parameters = ((AnimatorController)m_animator.runtimeAnimatorController).parameters.Select(x => x.name)
                .ToArray();

            if (target is IHasAnimationData && m_parameters.Length > 0)
            {
                SerializedProperty initializeStateNameEum = serializedObject.FindProperty("m_animatorParamName");
                initializeStateNameEum.stringValue = m_parameters[m_index];
            }
        }
    }

    private Animator FindPrefabsWithObjectReference(Object target, ref AnimationController animationController)
    {
        var result = new Animator();

        string[] allPrefabs = AssetDatabase.FindAssets("t:Prefab");
        foreach (var prefab in allPrefabs)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefab);
            GameObject prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            // Use GetComponentsInChildren to get all components in the prefab
            var components = prefabObject.GetComponentsInChildren<GuardNPCMono>();

            foreach (var component in components)
            {
                // Use SerializedObject and SerializedProperty to iterate over all serialized properties
                var serializedObject = new SerializedObject(component);
                var prop = serializedObject.GetIterator();

                while (prop.NextVisible(true))
                {
                    // If the property is an object reference, and the reference is the target object, add the prefab to the list
                    if (prop.propertyType == SerializedPropertyType.ObjectReference && prop.objectReferenceValue == target)
                    {
                        m_npc = component;
                        animationController = component.AnimationController;
                        result = component.AnimationController.Animator;
                        break;
                    }
                }
            }
        }

        return result;
    }
    
}
