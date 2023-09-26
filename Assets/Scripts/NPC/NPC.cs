using System.Collections.Generic;
using MonoBehaviours.Behaviours;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class NPC<T> : Character
{
    [FormerlySerializedAs("initializeStateNameEum")] [SerializeField]
    protected NPCStateNameEum m_initializeStateNameEum;
    [SerializeField]
    protected List<NpcBehaviorBase<T>> NpcStates;

    public NPCStateNameEum CurrentStateName;
    protected StateMachine StateMachine;

    public void ChangeState(NPCStateNameEum npcStateNameEum)
    {
        if(CurrentStateName == npcStateNameEum)
            StateMachine.CurrentState.OnEnter();
        
        StateMachine.ChangeState(GetStateByName(npcStateNameEum));
    }
    
    protected IBaseState GetStateByName(NPCStateNameEum npcStateNameEum)
    {
        CurrentStateName = npcStateNameEum;
        return NpcStates.Find(x => Equals(x.StateName, npcStateNameEum));
    }
}
