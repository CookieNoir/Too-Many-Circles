using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(MaskableGraphic))]
public class BlackScreen : MonoBehaviour
{
    private MaskableGraphic screen;
    private float intensity;
    private const float modifier = 0.5f;
    private Color black;

    private void Start()
    {
        screen = GetComponent<MaskableGraphic>();
        intensity = 1f;
        black = new Color(0f, 0f, 0f, 1f);
    }

    private void Update()
    {
        if (intensity > 0f)
        {
            intensity -= Time.deltaTime * modifier;
            black.a = Helper.SmoothStep(intensity);
            screen.color = black;
        }
        else Destroy(gameObject);
    }
}
