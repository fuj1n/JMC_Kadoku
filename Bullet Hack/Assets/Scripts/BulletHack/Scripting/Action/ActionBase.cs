using BulletHack.Scripting.Action.BlockAction;
using BulletHack.UI.BlockManager;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

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

        [System.AttributeUsage(System.AttributeTargets.Field)]
        [MeansImplicitUse]
        public sealed class InputVarAttribute : System.Attribute
        {
            public bool Reverse { get; set; }
        }
    }
}