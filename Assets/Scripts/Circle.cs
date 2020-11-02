using System;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Circle : FadeableObject
{
    public float radius;
    public Vector2 radiusRange;
    [Min(0.02f)] public float thickness;
    public Color color;
    private MeshFilter meshFilter;
    private Vector3[] pointsPositions;
    private Mesh mesh;

    protected virtual void Start()
    {
        CreateAndSetMesh();
    }

    protected void CreateAndSetMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        pointsPositions = new Vector3[UniversalCircle.PointsCountWithSecondCircle];

        mesh = new Mesh();
        Remesh();
        mesh.uv = UniversalCircle.uvs;
        mesh.triangles = UniversalCircle.triangles;
        meshFilter.mesh = mesh;
    }

    public virtual void ChangeRadius(float delta)
    {
        delta = radius + delta;
        if (!Mathf.Approximately(radius, delta))
        {
            radius = Mathf.Clamp(delta, radiusRange.x, radiusRange.y);
            Remesh();
        }
    }

    protected void Remesh()
    {
        Array.Copy(UniversalCircle.pointsPositions, pointsPositions, UniversalCircle.PointsCountWithSecondCircle);
        for (int i = UniversalCircle.PointsCount, j = UniversalCircle.PointsCountWithSecondCircle - 1; i > -1; --i, --j)
        {
            pointsPositions[i] *= radius - thickness;
            pointsPositions[j] *= radius + thickness;
        }
        mesh.vertices = pointsPositions;
    }

    private void OnValidate()
    {
        radius = Mathf.Clamp(radius, radiusRange.x, radiusRange.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusRange.x);
        Gizmos.DrawWireSphere(transform.position, radiusRange.y);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
