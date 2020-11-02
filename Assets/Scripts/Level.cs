using UnityEngine;

public class Level : MonoBehaviour
{
    public Circle final;
    public Circle[] circles;
    public FadeableObject[] fadeableObjects;

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
}
