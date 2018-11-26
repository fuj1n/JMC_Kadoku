using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CodeBlockDrag : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
    public bool cloneDrag;

    public RectTransform root;

    public bool undeletable;

    public Color outlineHover;
    public Color outlineDelete;
    public float fadeTime = .5F;

    private RectTransform rect;
    private Outline outline;

    private EventSystem system;

    private Transform inAnchor;
    private Transform target;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        outline = GetComponent<Outline>();
        outline.DOFade(0F, 0F);

        system = FindObjectOfType<EventSystem>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        target = null;

        if (cloneDrag)
        {
            Instantiate(gameObject, transform.parent).name = gameObject.name;
            system.SetSelectedGameObject(null, eventData);
            cloneDrag = false;
        }

        transform.SetParent(root, true);
        transform.localScale = Vector3.one;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!undeletable && eventData.hovered.Any(x => x && x.CompareTag("UI-Bin")))
        {
            outline.DOColor(outlineDelete, fadeTime);
        }
        else
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

        if (target)
        {
            // Prevent snapping when there's already a block there
            foreach (Transform t in target)
                if (t.CompareTag("UI-Block"))
                    return;

            rect.SetParent(target);

            Transform anchor = target.Find("Anchor");
            if (!anchor)
                anchor = target;

            rect.position += anchor.position - inAnchor.position;
        }
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

    public void SetTargetBlock(Transform inAnchor, Transform t)
    {
        this.inAnchor = inAnchor;
        target = t;
    }
}
