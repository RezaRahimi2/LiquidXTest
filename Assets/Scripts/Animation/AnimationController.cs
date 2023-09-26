using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private List<AnimatorParamData> m_AnimatorParamDatas;
    [SerializeField] private Animator m_animator;
    public Animator Animator => m_animator;

    public void Initialize(List<AnimatorParamData> animatorParamDatas)
    {
        m_AnimatorParamDatas = animatorParamDatas;
    }
    
    public int GetParameterHashByName(string parameterName)
    {
        AnimatorParamData? animationData = m_AnimatorParamDatas.Find(x => x.ParameterName == parameterName);

        if (animationData == null)
            throw new NullReferenceException("Animation Data not find");

        return animationData.Value.ParameterHash;
    }

    public void ChangeAnimation(string parameterName,object value)
    {
        var animationData = m_AnimatorParamDatas.Find(x => x.ParameterName == parameterName);
        switch (animationData.AnimatorParameterType)
        {
            case AnimatorControllerParameterType.Float:
                m_animator.SetFloat(animationData.ParameterHash,(float)value);
                break;
            case AnimatorControllerParameterType.Bool:
                m_animator.SetBool(animationData.ParameterHash,(bool)value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
}
