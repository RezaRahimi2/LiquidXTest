using UnityEngine;

public class WayPointsController : MonoBehaviour
{
    public GuardNPCMono GuardNpcMono;
    public Transform StaticWayPointParent;
    public string NpcName;

    public void UpdateWps()
    {
        if (StaticWayPointParent != null)
        {
            GuardNpcMono.UpdateWPs(StaticWayPointParent);
        }
    }
}
