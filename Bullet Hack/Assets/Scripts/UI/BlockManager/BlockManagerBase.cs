using UnityEngine;

public class BlockManagerBase : MonoBehaviour
{
    public RectTransform outAnchor;

    protected RectTransform rect;

    [HideInInspector]
    public BlockManager outConnector;

    protected virtual void Start()
    {
        rect = GetComponent<RectTransform>();
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
}
