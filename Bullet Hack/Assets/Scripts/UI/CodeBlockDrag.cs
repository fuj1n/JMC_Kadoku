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

    private BlockManagerBase blockManager;

    private EventSystem system;

    private Transform inAnchor;
    private Transform target;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        blockManager = GetComponent<BlockManager>();

        outline = GetComponent<Outline>();
        outline.DOFade(0F, 0F);

        system = FindObjectOfType<EventSystem>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CombatManager.Instance.Script.IsRunning)
            return;

        target = null;

        if (cloneDrag)
        {
            Instantiate(gameObject, transform.parent).name = gameObject.name;
            system.SetSelectedGameObject(null, eventData);
            cloneDrag = false;
        }

        if (blockManager is BlockManager)
            ((BlockManager)blockManager).DisconnectParent();

        transform.SetParent(root, true);
        transform.localScale = Vector3.one;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CombatManager.Instance.Script.IsRunning)
            return;

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
        if (CombatManager.Instance.Script.IsRunning)
            return;

        if (!undeletable && eventData.hovered.Any(x => x.CompareTag("UI-Bin")))
            Destroy(gameObject);
        else if (!eventData.hovered.Contains(gameObject))
            outline.DOFade(0F, fadeTime);

        if (target && blockManager is BlockManager)
        {
            BlockManagerBase bm = target.GetComponentInParent<BlockManagerBase>();
            if (!bm)
                return;

            RectTransform anchor = bm.Connect(target, (BlockManager)blockManager);

            if (!anchor)
                return;

            rect.SetParent(target);

            rect.position += anchor.position - inAnchor.position;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CombatManager.Instance.Script.IsRunning)
            return;

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
