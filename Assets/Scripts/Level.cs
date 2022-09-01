using UnityEngine;

public class Level : MonoBehaviour
{
    public Circle start;
    public Circle final;
    [Space(10, order = 0)]
    public float levelSpeed = 0.5f;
    [Range(0f, 360f, order = 1)] public float startAngle;
    public bool moveClockwise;
    [Space(10)]
    public Circle[] circles;
    public FadeableObject[] fadeableObjects;
    public Color colorStart;
    public Color colorEnd;
    public Color foregroundColorStart;
    public Color foregroundColorEnd;

    public void Hide()
    {
        for (int i = 0; i < circles.Length; ++i)
        {
            circles[i].Hide();
        }
        for (int i = 0; i < fadeableObjects.Length; ++i)
        {
            fadeableObjects[i].Hide();
        }
    }

    public void ShowCircles()
    {
        for (int i = 0; i < circles.Length; ++i)
        {
            circles[i].Show();
        }
    }

    public void ShowFadeableObjects()
    {
        for (int i = 0; i < fadeableObjects.Length; ++i)
        {
            fadeableObjects[i].Show();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (start)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(start.transform.position + 
                PolarCoordinateSystem.GetPosition(startAngle * Mathf.Deg2Rad, start.radius), 0.2f);
        }
    }
}
