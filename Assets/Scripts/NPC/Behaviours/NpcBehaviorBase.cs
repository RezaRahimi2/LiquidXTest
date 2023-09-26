using UnityEngine;

namespace MonoBehaviours.Behaviours
{
    public abstract class NpcBehaviorBase<T> :ScriptableObject,IBaseStateWithName<NPCStateNameEum>
    {
        public bool IsWaiting { get; set; }

        [field:SerializeField]
        public int UpdateRate { get; set; }
        
        [field:SerializeField]
        public NPCStateNameEum StateName { get; set; }
        
        protected T NPC { get; private set; }

        public virtual void Initialize(T _npc)
        {
            this.NPC = _npc;
        }
        
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public abstract void Tick();

    }
}