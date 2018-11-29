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
            nameText.text = GetName();
    }

    public virtual void ResetState()
    {

    }

    public virtual BlockManagerBase GetManager() => manager;

    public abstract string GetName();

    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class InputVarAttribute : System.Attribute{}
}
