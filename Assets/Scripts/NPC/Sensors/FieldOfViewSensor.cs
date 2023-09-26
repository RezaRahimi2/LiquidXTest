using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public sealed class FieldOfViewSensor : SensorBase
{
    [SerializeField] private LayerMask PlayerLayerMask;
    [SerializeField] private bool ShowMesh;
    [SerializeField] private FieldOfViewRenderer m_fieldOfViewRenderer;
    public float farViewDistance;
    public float nearViewDistance;
    
    // Define the view angle and resolution
    public float viewAngle;
    public float resolution = 10;
    private DetectData m_detectData = new DetectData();
    private Coroutine m_lostPlayerCoroutine;
    Transform m_playerTransform;

   public void Start()
    {
        if (ShowMesh)
        {
            m_fieldOfViewRenderer?.Initialize(this);
            Logic();
        }
        else
            Destroy(m_fieldOfViewRenderer);
        
        CustomUpdate();
    }

   protected override void Logic()
   {
        List<Vector3> nearViewPoints = CalculateViewPoints();
        if (ShowMesh)
        {
            m_fieldOfViewRenderer.DrawNearFieldOfView(nearViewPoints);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
    List<Vector3> CalculateViewPoints()
    {
        float stepCount = resolution * viewAngle;
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        bool m_hitToPlayer = false;
        
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;

            Vector3 dir = DirFromAngle(angle, true);
            Vector3 viewPoint = transform.position + DirFromAngle(angle, true) * farViewDistance;
            
            RaycastHit hit;
            if (Physics.Raycast( transform.position, dir, out hit, farViewDistance,DetectionLayerMask))
            {
                if (LayerMaskUtility.CheckLayer(hit.transform.gameObject.layer, PlayerLayerMask) &&
                    OnDetectEvents != null)
                {
                    if (Vector3.Dot(transform.forward, (hit.point - transform.position).normalized) > 0.5f)
                    {
                        if (Vector3.Distance(transform.position, hit.point) < nearViewDistance &&
                            OnDetectEvents.ContainsKey(SensorEventName.OnNearDetect))
                        {
                            m_detectData.GameObjectData = hit.transform.gameObject;
                            OnDetectEvents[SensorEventName.OnNearDetect]?.Invoke(m_detectData);
                            if(m_lostPlayerCoroutine != null)
                                StopCoroutine(m_lostPlayerCoroutine);
                            Debug.Log($"Hit with {hit.transform.name} in nearViewDistance");
                        }
                        else if (OnDetectEvents.Count > 0 && OnDetectEvents.ContainsKey(SensorEventName.OnFarDetect))
                        {
                            m_detectData.VectorData = hit.point;
                            OnDetectEvents[SensorEventName.OnFarDetect]?.Invoke(m_detectData);
                            if(m_lostPlayerCoroutine != null)
                                StopCoroutine(m_lostPlayerCoroutine);
                            Debug.Log($"Hit with {hit.transform.name} in farViewDistance");
                        }

                        m_playerTransform = hit.transform;
                        LastPlayerDetectPosition = hit.point;
                        m_hitToPlayer = true;
                    }
                }

                viewPoint = hit.point;
            }
            
            viewPoints.Add(viewPoint);
        }

        if (PlayerIsInRange && !m_hitToPlayer && Vector3.Distance(transform.position ,m_playerTransform.position) > nearViewDistance)
        {
            m_lostPlayerCoroutine = StartCoroutine(LostPlayer());
        }
        PlayerIsInRange = m_hitToPlayer;
        
        return viewPoints;
    }

    IEnumerator LostPlayer()
    {
        yield return new WaitForSeconds(.5f);
        m_detectData.VectorData = LastPlayerDetectPosition;
        OnDetectEvents[SensorEventName.OnLostPlayer]?.Invoke(m_detectData);
        Debug.Log($"Lost player in {LastPlayerDetectPosition}");
    }
}