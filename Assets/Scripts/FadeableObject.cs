using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Renderer))]
public class FadeableObject : MonoBehaviour
{
    [Range(0f, 1f)] public float startAlpha;
    private IEnumerator fade;
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        fade = Fade();
        material.SetFloat("_AlphaMultiplier", startAlpha);
    }

    public void Hide()
    {
        StopCoroutine(fade);
        fade = Fade();
        StartCoroutine(fade);
    }

    public void Show()
    {
        StopCoroutine(fade);
        fade = Appear();
        StartCoroutine(fade);
    }

    private IEnumerator Appear()
    {
        float time = 0f;
        while (time < 1f)
        {
            material.SetFloat("_AlphaMultiplier", Helper.SmoothStep(time));
            yield return null;
            time += Time.deltaTime;
        }
        material.SetFloat("_AlphaMultiplier", 1f);
    }

    private IEnumerator Fade()
    {
        float time = 1f;
        while (time > 0f)
        {
            material.SetFloat("_AlphaMultiplier", Helper.SmoothStep(time));
            yield return null;
            time -= Time.deltaTime;
        }
        material.SetFloat("_AlphaMultiplier", 0f);
    }
}
