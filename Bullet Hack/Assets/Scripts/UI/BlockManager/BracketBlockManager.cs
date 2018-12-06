﻿using UnityEngine;

public class BracketBlockManager : BlockManager
{
    public RectTransform bracketAnchor;

    [HideInInspector]
    public BlockManager bracketConnector;

    private float minSize;

    protected override void Start()
    {
        base.Start();

        minSize = rect.sizeDelta.y;
    }

    public override void OnHierarchyChanged()
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, minSize + ComputeSize(bracketConnector));

        base.OnHierarchyChanged();
    }

    public override RectTransform Connect(Transform anchor, BlockManager block)
    {
        if (anchor != bracketAnchor.parent)
            return base.Connect(anchor, block);

        bracketConnector = block;
        block.inConnector = this;

        OnHierarchyChanged();

        return bracketAnchor;
    }

    private float ComputeSize(BlockManager manager)
    {
        if (!manager)
            return 0F;

        return manager.GetSize() + ComputeSize(manager.outConnector);
    }

    public override void Disconnect(BlockManager child)
    {
        if (bracketConnector == child)
        {
            bracketConnector.inConnector = null;
            bracketConnector = null;

            OnHierarchyChanged();
        }
        else
            base.Disconnect(child);
    }

    public BlockManager GetBracketConnection()
    {
        return bracketConnector;
    }

    public override void SetOutline(Color c, float time, bool setAlpha = true, bool propagate = false)
    {
        base.SetOutline(c, time, setAlpha, propagate);

        if (propagate && bracketConnector)
            bracketConnector.SetOutline(c, time, setAlpha, propagate);
    }

    public override void FadeOutline(float f, float time, bool propagate = false)
    {
        base.FadeOutline(f, time, propagate);

        if (propagate && bracketConnector)
            bracketConnector.FadeOutline(f, time, propagate);
    }
}
