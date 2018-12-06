using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CodeBlockDrag : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
    public bool cloneDrag;

    public RectTransform root;

    public bool undeletable;

    public Color outlineHover;
    public Color outlineDelete;
    public float fadeTime = .5F;

    private RectTransform rect;

    private BlockManagerBase blockManager;

    private EventSystem system;

    private Transform inAnchor;
    private Transform target;

    public ValueBinder[] binders;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        blockManager = GetComponent<BlockManagerBase>();

        system = FindObjectOfType<EventSystem>();
    }

    private void Start()
    {
        binders = GetComponentsInChildren<ValueBinder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CombatManager.Instance.Script.IsRunning)
            return;

        target = null;

        if (cloneDrag)
        {
            GameObject go = Instantiate(gameObject, transform.parent);
            go.name = gameObject.name;

            go.GetComponent<CodeBlockDrag>().UpdateBinders(binders);

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
            blockManager.SetOutline(outlineDelete, fadeTime, propagate: true);
        }
        else
        {
            blockManager.SetOutline(outlineHover, fadeTime, propagate: true);
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
            blockManager.FadeOutline(0F, fadeTime, true);

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

        if (eventData.pointerCurrentRaycast.gameObject != gameObject)
            return;

        blockManager.SetOutline(outlineHover, 0F, false, true);
        blockManager.FadeOutline(1F, fadeTime, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging && eventData.pointerDrag == gameObject)
            return;

        blockManager.FadeOutline(0F, fadeTime, true);
    }

    public void SetTargetBlock(Transform inAnchor, Transform t)
    {
        this.inAnchor = inAnchor;
        target = t;
    }

    private void UpdateBinders(ValueBinder[] binds)
    {
        for (int i = 0; i < binders.Length; i++)
        {
            binders[i].field = binds[i].field;
            binders[i].obj = GetComponent<ActionBase>();
        }
    }
}
