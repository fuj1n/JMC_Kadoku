using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CodeBlockDrag : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
    public bool cloneDrag;
    [ConditionalHide(true, ConditionalSourceField = "cloneDrag")]
    public RectTransform cloneTarget;

    public bool undeletable;

    public Color outlineHover;
    public Color outlineDelete;
    public float fadeTime = .5F;

    private RectTransform rect;
    private Outline outline;

    private EventSystem system;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        outline = GetComponent<Outline>();
        outline.DOFade(0F, 0F);

        system = FindObjectOfType<EventSystem>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (cloneDrag && cloneTarget)
        {
            Instantiate(gameObject, transform.parent).name = gameObject.name;
            transform.SetParent(cloneTarget, true);
            transform.localScale = Vector3.one;
            system.SetSelectedGameObject(null, eventData);
            cloneDrag = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!undeletable && eventData.hovered.Any(x => x.CompareTag("UI-Bin")))
        {
            outline.DOColor(outlineDelete, fadeTime);
        } else
        {
            outline.DOColor(outlineHover, fadeTime);
        }

        rect.anchoredPosition += eventData.delta / rect.parent.lossyScale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!undeletable && eventData.hovered.Any(x => x.CompareTag("UI-Bin")))
            Destroy(gameObject);
        else if (!eventData.hovered.Contains(gameObject))
            outline.DOFade(0F, fadeTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color c = outlineHover;
        c.a = outline.effectColor.a;
        outline.effectColor = c;
        outline.DOFade(1F, fadeTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging && eventData.pointerDrag == gameObject)
            return;

        outline.DOFade(0F, fadeTime);
    }
}
