using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer),typeof(MeshFilter))]
public class FieldOfViewRenderer : MonoBehaviour
{
    private FieldOfViewSensor _controller;
    
    private LayerMask PlayerMask;

   // Define two different materials for near and far field of view
    public Material nearMaterial;

    // Define two different meshes for near and far field of view
    private Mesh nearViewMesh;

    // Define MeshFilter and MeshRenderer for near and far field of view
    private MeshFilter nearMeshFilter;
    private MeshRenderer nearMeshRenderer;

    public void Initialize(FieldOfViewSensor fieldOfViewSensor)
    {
        _controller = fieldOfViewSensor;
        nearViewMesh = new Mesh();

        // Initialize MeshFilter and MeshRenderer for near field of view
        nearMeshFilter = GetComponent<MeshFilter>();
        nearMeshRenderer = GetComponent<MeshRenderer>();
        nearMeshRenderer.material = nearMaterial;
    }
    
    public void DrawNearFieldOfView(List<Vector3> viewPoints)
    {
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0.5f, 0.5f);

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            // Calculate the direction from the GuardNpc to the vertex
            Vector3 directionToVertex = (viewPoints[i] - transform.position).normalized;

            // Calculate the angle between the forward direction and the direction to the vertex
            float angle = Vector3.SignedAngle(transform.forward, directionToVertex, transform.up);

            // Normalize the angle to the range [0, 1]
            float u = angle / 360f + 0.5f;

            // Calculate the distance from the GuardNpc to the vertex
            float distanceToVertex = Vector3.Distance(transform.position, viewPoints[i]);

            // Normalize the distance to the range [0, 1]
            float v = distanceToVertex / _controller.farViewDistance; // maxViewDistance is the maximum distance that the GuardNpc can see

            // Map the UVs based on the polar coordinates of the vertices
            uv[i + 1] = new Vector2(u, v);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        nearViewMesh.Clear();
        nearViewMesh.vertices = vertices;
        nearViewMesh.uv = uv;
        nearViewMesh.triangles = triangles;
        nearViewMesh.RecalculateNormals();

        nearMeshFilter.mesh = nearViewMesh;
    }
}
