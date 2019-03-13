using System;
using BulletHack.Scripting.Action.BlockAction;
using BulletHack.UI.Blocks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHack.Scripting.Action
{
    public abstract class ActionBase : MonoBehaviour
    {
        public TextMeshProUGUI nameText;

        protected BlockManagerBase manager;

        public abstract void Execute();
        public virtual ActionBase GetNextAction() => GetNextActionRaw()?.GetComponent<ActionBase>();
        public virtual BlockManagerBase GetNextActionRaw() => manager.GetOutConnection();

        public bool doDownscale = true;

        public string Id { get; private set; }

        private void Awake()
        {
            manager = GetComponent<BlockManagerBase>();
        }

        protected virtual void Start()
        {
            if (nameText)
            {
                nameText.text = GetName();

                nameText.ForceMeshUpdate();

                if (doDownscale)
                    DownscaleByText();
                doDownscale = false;
            }

            if (GetColor() != default)
                GetComponent<Graphic>().color = GetColor();
        }

        public virtual void ResetState()
        {
            manager.GetOutConnection()?.GetComponent<ActionBlockBase>()?.ResetState();
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
        public virtual Color GetColor() => default;

        public void SetId(string id)
        {
            if (Id != null)
                throw new ArgumentException("Attempted to set action ID twice");

            Id = id;
        }

        [AttributeUsage(AttributeTargets.Field)]
        [MeansImplicitUse(ImplicitUseKindFlags.Assign, ImplicitUseTargetFlags.WithMembers)]
        public sealed class InputVarAttribute : Attribute
        {
            public bool Reverse { get; set; }
        }
    }
}