using UnityEngine;

public class ChainedCircle : Circle
{
    [Header("Chaining")]
    [Min(0f)] public float chainModifier;
    public Circle[] circles;
    public LineRenderer[] lineRenderers;
    public float scaleMultiplier;
    private float[] radiuses;


    protected override void Start()
    {
        radiuses = new float[circles.Length];

        CalculateRadius();
        CreateAndSetMesh();
    }

    public override void ChangeRadius(float delta)
    {
        // It's empty because we change chained circle's radius by another circles
    }

    private void CalculateRadius()
    {
        radius = 0;
        for (int i = 0; i < circles.Length; ++i)
        {
            radius += circles[i].radius * chainModifier;
            radiuses[i] = circles[i].radius;
        }
        for (int i = 0; i < circles.Length; ++i)
        {
            Vector3 position1 = circles[i].transform.position + (transform.position - circles[i].transform.position).normalized * radiuses[i];
            Vector3 position2 = transform.position + (circles[i].transform.position - transform.position).normalized * radius;

            lineRenderers[i].SetPosition(0, position1);
            lineRenderers[i].SetPosition(1, position2);
            lineRenderers[i].material.mainTextureScale = new Vector2(Vector3.Magnitude(position1 - position2)*scaleMultiplier, 1f);
        }
        radius = Mathf.Clamp(radius, radiusRange.x, radiusRange.y);
    }

    private void RecalculateRadius()
    {
        CalculateRadius();
        Remesh();
    }

    private void CheckForChanges()
    {
        for (int i = 0; i < circles.Length; ++i)
        {
            if (radiuses[i] != circles[i].radius)
            {
                RecalculateRadius();
            }
        }
    }

    private void Update()
    {
        CheckForChanges();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusRange.x);
        Gizmos.DrawWireSphere(transform.position, radiusRange.y);
        Gizmos.color = Color.red;

        float rad = 0;
        for (int i = 0; i < circles.Length; ++i)
        {
            rad += circles[i].radiusRange.y * chainModifier;
        }
        Gizmos.DrawWireSphere(transform.position, rad);
    }
}
