using System;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Circle : FadeableObject
{
    public float radius;
    public Vector2 radiusRange;
    [Range(0f, 1f)] public float colorGradientValue;
    public float thickness = 0.025f;
    private MeshFilter meshFilter;
    private Vector3[] pointsPositions;
    private Mesh mesh;
    protected float clippedValue;

    protected override void Awake()
    {
        base.Awake();
        CreateAndSetMesh();
        ClipRadius();
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

    public virtual void SetRadius(float newRadius)
    {
        radius = Mathf.Clamp(newRadius, radiusRange.x, radiusRange.y);
        Remesh();
        ClipRadius();
    }

    public virtual void ChangeRadius(float delta)
    {
        delta = radius + delta;
        if (!Mathf.Approximately(radius, delta))
        {
            radius = Mathf.Clamp(delta, radiusRange.x, radiusRange.y);
            Remesh();
            ClipRadius();
        }
    }

    protected virtual void ClipRadius()
    {
        clippedValue = Mathf.Approximately(radiusRange.x, radiusRange.y) ? -1f : (radius - radiusRange.x) / (radiusRange.y - radiusRange.x);
    }

    public float GetClippedValue()
    {
        return clippedValue;
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
