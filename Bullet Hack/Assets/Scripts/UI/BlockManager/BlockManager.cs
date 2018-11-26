using UnityEngine;

public class BlockManager : BlockManagerBase
{
    public RectTransform inAnchor;

    [HideInInspector]
    public BlockManagerBase inConnector;

    public override void OnHierarchyChanged()
    {
        base.OnHierarchyChanged();

        if (inConnector)
            inConnector.OnHierarchyChanged();
    }

    public virtual void DisconnectParent()
    {
        if (inConnector)
            inConnector.Disconnect(this);
    }
}
