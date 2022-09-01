using UnityEngine;

public static class Helper
{
    public static float SmoothStep(float value)
    {
        return value * value * (3f - 2f * value);
    }

    public static Vector2 ClipPixelPosition(Vector2 pixelPosition)
    {
        return new Vector2(pixelPosition.x / Screen.width, pixelPosition.y / Screen.height);
    }
}
