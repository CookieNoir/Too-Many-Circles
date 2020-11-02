using UnityEngine;

public class UniversalCircle : MonoBehaviour
{
    public const int PointsCount = 64;
    public const float PointsCountInverted = 0.015625f;
    public const int PointsCountWithSecondCircle = 130; // (PointsCount + 1) * 2. Another point is added for fine looking UV unwrap

    public static Vector3[] pointsPositions; // Calculated ONCE for the circle with radius = 1 and duplicated for the second circle
    public static Vector2[] uvs;
    public static int[] triangles;

    private void Awake()
    {
        SetUniversalCircle();
    }

    private void SetUniversalCircle()
    {
        pointsPositions = new Vector3[PointsCountWithSecondCircle];
        uvs = new Vector2[PointsCountWithSecondCircle];
        triangles = new int[PointsCount * 6];
        int topIndex = PointsCountWithSecondCircle - 1, p0 = PointsCount, p1 = topIndex, triangleIndex = PointsCount * 6;
        float offset = 1f;
        Vector3 vector3 = new Vector3(1f, 0f);

        pointsPositions[PointsCount] = vector3;
        uvs[PointsCount] = new Vector2(offset, 0f);

        pointsPositions[topIndex] = vector3;
        uvs[topIndex] = new Vector2(offset, 1f);

        for (int i = PointsCount - 1; i > -1; --i)
        {
            offset -= PointsCountInverted;
            vector3 = PolarCoordinateSystem.GetPosition(offset * PolarCoordinateSystem.DualPI, 1f);
            pointsPositions[i] = vector3;
            uvs[i] = new Vector2(offset, 0f);

            triangleIndex -= 6;
            triangles[triangleIndex] = p0;
            triangles[triangleIndex + 1] = p1;
            triangles[triangleIndex + 2] = i;
            p0 = i;

            topIndex--;
            pointsPositions[topIndex] = vector3;
            uvs[topIndex] = new Vector2(offset, 1f);

            triangles[triangleIndex + 3] = p0;
            triangles[triangleIndex + 4] = p1;
            triangles[triangleIndex + 5] = topIndex;
            p1 = topIndex;
        }
    }
}
