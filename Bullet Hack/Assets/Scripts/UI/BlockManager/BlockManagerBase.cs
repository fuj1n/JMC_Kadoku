using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BlockManagerBase : MonoBehaviour
{
    public RectTransform outAnchor;

    protected RectTransform rect;

    [HideInInspector]
    public BlockManager outConnector;

    private Outline outline;

    public virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        outline = GetComponent<Outline>();

        FadeOutline(0F, 0F);
    }

    public virtual void OnHierarchyChanged()
    {

    }

    public virtual float GetSize()
    {
        return rect.sizeDelta.y - 31.5F;
    }

    public virtual RectTransform Connect(Transform anchor, BlockManager block)
    {
        if (anchor != outAnchor.parent || outConnector)
            return null;

        outConnector = block;
        block.inConnector = this;

        OnHierarchyChanged();

        return outAnchor;
    }

    public virtual void Disconnect(BlockManager child)
    {
        if (outConnector == child)
        {
            outConnector.inConnector = null;
            outConnector = null;

            OnHierarchyChanged();
        }
    }

    public virtual BlockManager GetOutConnection()
    {
        return outConnector;
    }

    public virtual void SetOutline(Color c, float time, bool setAlpha = true, bool propagate = false)
    {
        if (!setAlpha)
            c.a = outline.effectColor.a;

        outline.DOColor(c, time);

        if (propagate && outConnector)
            outConnector.SetOutline(c, time, propagate);
    }

    public virtual void FadeOutline(float f, float time, bool propagate = false)
    {
        outline.DOFade(f, time);

        if (propagate && outConnector)
            outConnector.FadeOutline(f, time, propagate);
    }
}
