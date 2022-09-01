using UnityEngine;

public class ChainedCircle : Circle
{
    [Header("Chaining")]
    public float startRadius;
    public float chainModifier;
    public Circle[] circles;
    public LineRenderer[] lineRenderers;
    private float[] radiuses;

    protected override void Awake()
    {
        base.Awake();
        radiuses = new float[circles.Length];
    }

    protected void Start()
    {
        CalculateRadius();
        CreateAndSetMesh();
        ClipRadius();
    }

    public override void ChangeRadius(float delta)
    {
        // It's empty because we change chained circle's radius by another circles
    }

    private void CalculateRadius()
    {
        radius = startRadius;
        for (int i = 0; i < circles.Length; ++i)
        {
            radius += circles[i].radius * chainModifier;
            radiuses[i] = circles[i].radius;
        }
        for (int i = 0; i < circles.Length; ++i)
        {
            if (lineRenderers[i])
            {
                Vector3 position1 = circles[i].transform.position + (transform.position - circles[i].transform.position).normalized * radiuses[i];
                Vector3 position2 = transform.position + (circles[i].transform.position - transform.position).normalized * radius;

                lineRenderers[i].SetPosition(0, position1);
                lineRenderers[i].SetPosition(1, position2);
                lineRenderers[i].material.mainTextureScale = new Vector2(Vector3.Magnitude(position1 - position2) * GameController.materialScaleMultiplier, 1f);
            }
        }
        radius = Mathf.Clamp(radius, radiusRange.x, radiusRange.y);
    }

    protected override void ClipRadius()
    {
        clippedValue = -1f;
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
        float maxRadius = startRadius;
        float minRadius = startRadius;
        float currentRadius = startRadius;
        for (int i = 0; i < circles.Length; ++i)
        {
            maxRadius += circles[i].radiusRange.y * chainModifier;
            minRadius += circles[i].radiusRange.x * chainModifier;
            currentRadius += circles[i].radius * chainModifier;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, currentRadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, minRadius);
    }
}
