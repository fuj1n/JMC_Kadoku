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

    protected virtual void Start()
    {
        rect = GetComponent<RectTransform>();
        outline = GetComponent<Outline>();
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

    public void SetOutline(Color c, float time)
    {
        outline.DOColor(c, time);
    }
}
