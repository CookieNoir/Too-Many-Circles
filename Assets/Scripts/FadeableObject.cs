using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Renderer))]
public class FadeableObject : MonoBehaviour
{
    [Range(0f, 1f)] public float startAlpha;
    private IEnumerator fade;
    private Material material;
    private Color _startColor;

    protected virtual void Awake()
    {
        material = GetComponent<Renderer>().material;
        fade = Fade();
        _startColor = material.GetColor("_Color");
        _startColor.a = startAlpha;
        material.SetColor("_Color", _startColor);
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
            _startColor.a = Helper.SmoothStep(time);
            material.SetColor("_Color", _startColor);
            yield return null;
            time += Time.deltaTime;
        }
        _startColor.a = Helper.SmoothStep(1f);
        material.SetColor("_Color", _startColor);
    }

    private IEnumerator Fade()
    {
        float time = 1f;
        while (time > 0f)
        {
            _startColor.a = Helper.SmoothStep(time);
            material.SetColor("_Color", _startColor);
            yield return null;
            time -= Time.deltaTime;
        }
        _startColor.a = Helper.SmoothStep(0f);
        material.SetColor("_Color", _startColor);
    }
}
