﻿using TMPro;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    protected BlockManagerBase manager;

    public abstract void Execute();
    public virtual ActionBase GetNextAction() => GetNextActionRaw()?.GetComponent<ActionBase>();
    public virtual BlockManagerBase GetNextActionRaw() => manager.GetOutConnection();

    protected virtual void Start()
    {
        manager = GetComponent<BlockManagerBase>();

        if (nameText)
        {
            nameText.text = GetName();

            nameText.ForceMeshUpdate();

            DownscaleByText();
        }
    }

    public virtual void ResetState()
    {

    }

    public virtual BlockManagerBase GetManager() => manager;

    public virtual void DownscaleByText()
    {
        RectTransform rect = GetComponent<RectTransform>();

        RectTransform tRect = nameText.GetComponent<RectTransform>();
        float dx = tRect.sizeDelta.x - (nameText.preferredWidth + 20F);
        tRect.sizeDelta = new Vector2(nameText.preferredWidth, tRect.sizeDelta.y);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x - dx, rect.sizeDelta.y);
    }

    public abstract string GetName();

    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class InputVarAttribute : System.Attribute { }
}
