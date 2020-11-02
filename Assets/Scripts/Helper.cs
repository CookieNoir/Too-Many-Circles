public static class Helper
{
    public static float SmoothStep(float value)
    {
        return value * value * (3f - 2f * value);
    }
}
