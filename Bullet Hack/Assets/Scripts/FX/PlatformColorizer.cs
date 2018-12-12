using DG.Tweening;
using UnityEngine;

public class PlatformColorizer : MonoBehaviour
{
    public float brightenValue = 4F;
    public float fadeTime = .2F;

    private Color startColor;
    private Color endColor;

    private new SpriteRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        startColor = renderer.color;
        endColor = startColor * brightenValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        renderer.DOKill();
        renderer.DOColor(endColor, fadeTime);
    }

    private void OnTriggerExit(Collider other)
    {
        renderer.DOKill();
        renderer.DOColor(startColor, fadeTime);
    }
}
