using UnityEngine;

//this is a place holder of NPC move to a Target if not using NavMeshAgent in NPC
public class NPCMovement : MonoBehaviour
{
    public float movementSpeed = 2f; // Adjust the movement normalSpeed as needed
    private int currentWaypoint = 0;
    private GuardNPCMono _mGuardNpcMono;

    void Start()
    {
        _mGuardNpcMono = GetComponent<GuardNPCMono>();
    }

    void Update()
    {
        if (_mGuardNpcMono.CurrentTarget != default)
        {
            Vector3 targetPosition = _mGuardNpcMono.CurrentTarget;
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * movementSpeed * Time.deltaTime;

            // Rotate towards the current waypoint
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}