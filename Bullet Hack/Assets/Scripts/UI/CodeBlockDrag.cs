using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CodeBlockDrag : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{
    public Color outlineHover;
    public float fadeTime = .5F;

    private RectTransform rect;
    private Outline outline;

    private CanvasScaler scaler;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        outline = GetComponent<Outline>();
        outline.DOFade(0F, 0F);

        scaler = GetComponentInParent<CanvasScaler>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / scaler.transform.lossyScale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!eventData.hovered.Contains(gameObject))
            outline.DOFade(0F, fadeTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.effectColor = outlineHover;
        outline.DOFade(1F, fadeTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging && eventData.pointerDrag == gameObject)
            return;

        outline.DOFade(0F, fadeTime);
    }
}
