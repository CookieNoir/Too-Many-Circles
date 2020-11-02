using UnityEngine;

public class RadiusChanger
{
    private float lastX = 0f;
    private float delta = 0f;

    public float GetDelta()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            delta = (Input.mousePosition.x - lastX) / Screen.width;
            lastX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            delta = 0f;
        }
        return delta;
    }
}
