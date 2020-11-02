using System.Collections;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public float smoothTime = 0.3f;

    private Transform target;
    private Color targetColor = Color.green;
    private Vector3 velocity = Vector3.zero;
    private IEnumerator colorCoroutine;

    private void Awake()
    {
        colorCoroutine = ColorCoroutine();
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
    }

    public void SetTargetAndColor(Transform newTarget, Color newColor)
    {
        target = newTarget;
        targetColor = newColor;

        StopCoroutine(colorCoroutine);
        colorCoroutine = ColorCoroutine();
        StartCoroutine(colorCoroutine);
    }

    private IEnumerator ColorCoroutine()
    {
        Color startColor = Camera.main.backgroundColor;
        float time = 0f;
        while (time < 1f)
        {
            Camera.main.backgroundColor = Color.Lerp(startColor, targetColor, Helper.SmoothStep(time));
            yield return null;
            time += Time.deltaTime;
        }
        Camera.main.backgroundColor = targetColor;
    }
}
