using System.Collections;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public float smoothTime = 0.3f;
    public SpriteRenderer foreground;

    private Transform target;
    private Color targetColor1 = Color.green;
    private Color targetColor2 = Color.green;
    private Color targetForegroundColor1 = Color.gray;
    private Color targetForegroundColor2 = Color.gray;
    private Vector3 velocity = Vector3.zero;
    private IEnumerator colorCoroutine;

    private void Awake()
    {
        colorCoroutine = ColorCoroutine(Color.green, Color.white);
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
    }

    public void SetTargetAndGradientValue(Transform newTarget, float gradientValue)
    {
        target = newTarget;

        StopCoroutine(colorCoroutine);
        colorCoroutine = ColorCoroutine(Color.Lerp(targetColor1, targetColor2, gradientValue), 
            Color.Lerp(targetForegroundColor1, targetForegroundColor2, gradientValue));
        StartCoroutine(colorCoroutine);
    }

    public void SetColorGradient(Color newColor1, Color newColor2, Color newForegroundColor1, Color newForegroundColor2)
    {
        targetColor1 = newColor1;
        targetColor2 = newColor2;
        targetForegroundColor1 = newForegroundColor1;
        targetForegroundColor2 = newForegroundColor2;
    }

    private IEnumerator ColorCoroutine(Color targetColor, Color targetForegroundColor)
    {
        Color startColor = Camera.main.backgroundColor;
        Color foregroundStartColor = foreground.color;
        float time = 0f;
        while (time < 1f)
        {
            Camera.main.backgroundColor = Color.Lerp(startColor, targetColor, Helper.SmoothStep(time));
            foreground.color = Color.Lerp(foregroundStartColor, targetForegroundColor, Helper.SmoothStep(time));
            yield return null;
            time += Time.deltaTime;
        }
        Camera.main.backgroundColor = targetColor;
        foreground.color = targetForegroundColor;
    }
}
