using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonoBehaviours.NPCs.Editor
{
    [CustomEditor(typeof(Character),true)]
    public class NpcEditor : UnityEditor.Editor
    {
        private Character m_npc;
        private IHasAnimation m_anim;
        public override VisualElement CreateInspectorGUI()
        {
            m_npc = target as Character;
            m_anim = (target as IHasAnimation);
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            if (target is global::Player)
                base.DrawDefaultInspector();
                
            Type[] types = target.GetType().GetInterfaces();
            
           if(types == null || types.Length == 0 || types?.Contains(typeof(IHasAnimation)) == null)
                return;
           
           if(m_anim.AnimationController == null)
               EditorGUILayout.HelpBox("AnimationController field is empty",MessageType.Error);
           if (m_anim.AnimationDatas != null && m_anim.AnimationController.Animator == null)
               EditorGUILayout.HelpBox("Animator field in AnimationController is empty",MessageType.Error);
           else
               EditorGUILayout.HelpBox("Fill Animator Param Data List with Animator Parameters",MessageType.Info);
           
            if (GUILayout.Button("Fill Animator Param Data"))
            {
                Animator animator = m_anim.AnimationController.Animator;
                
                m_anim.AnimationDatas = new List<AnimatorParamData>();
                
                for (var i = 0; i < animator.parameters.Length; i++)
                {
                    m_anim.AnimationDatas.Add(new AnimatorParamData(animator.parameters[i].type,animator.parameters[i].nameHash,animator.parameters[i].name));
                }
                
                m_anim.AnimationController.Initialize(m_anim.AnimationDatas);
            }
        }
    }
}