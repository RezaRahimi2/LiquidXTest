using System;
using UnityEngine;

[Serializable]
public struct AnimatorParamData
{
    [SerializeField] public AnimatorControllerParameterType AnimatorParameterType;
    [SerializeField] public int ParameterHash;
    [SerializeField] public string ParameterName;

    public AnimatorParamData(AnimatorControllerParameterType animatorParameterType, int parameterHash, string parameterName)
    {
        AnimatorParameterType = animatorParameterType;
        ParameterHash = parameterHash;
        ParameterName = parameterName;
    }
}
