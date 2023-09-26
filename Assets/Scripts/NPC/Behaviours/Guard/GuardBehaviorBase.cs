using MonoBehaviours.Behaviours;
using MonoBehaviours.Interface;
using UnityEngine;
using UnityEngine.AI;

public abstract class GuardBehaviorBase : NpcBehaviorBase<GuardNPCMono>,IHasAnimationData
{
    protected NavMeshAgent m_navMeshAgent;
    [SerializeField] private string m_animatorParamName;

    #region AnimatorParamData

    public string AnimatorParamName
    {
        get => m_animatorParamName;
    }

    [field:SerializeField]
    public float AnimatorParamValue { get; set; }
    [field:SerializeField]
    public float Speed { get; set; }

    #endregion

        
}