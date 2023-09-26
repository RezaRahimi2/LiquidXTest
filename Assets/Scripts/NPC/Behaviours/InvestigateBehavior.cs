using UnityEngine;

namespace MonoBehaviours.Behaviours
{
    public class InvestigateBehavior : GuardBehaviorBase
    {
        public override void Tick()
        {
            if (Vector3.Distance(NPC.transform.position, NPC.LastPlayerSeenPosition) < 0.5f)
            {
                NPC.CurrentStateName = NPCStateNameEum.Patrolling;
            }
        }
    }
}