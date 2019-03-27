using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHack.UI
{
    public class PopupMessageUi : MonoBehaviour
    {
        [Header("Bindings")]
        public Image icon;
        public TextMeshProUGUI text;
        public Graphic[] colorables = {};
        public RectTransform expansionSource;

        private PopupData data;
        private OnClosed callback;

        private static PopupMessageUi instance;
        public delegate void OnClosed();
        
        private void Start()
        {
            if (icon)
            {
                if (!data.image)
                    icon.enabled = false;

                icon.sprite = data.image;
            }

            if (text)
                text.text = data.text;

            foreach (Graphic g in colorables)
            {
                if(data.color == default)
                    continue;
                Color c = data.color;
                c.a = g.color.a;
                g.color = c;
            }

            if (expansionSource)
                expansionSource.sizeDelta += data.expand;

            Time.timeScale = 0F;
        }

        public static void Open(PopupData data, OnClosed callback = null)
        {
            if(!POPUPS.ContainsKey(data.direction))
                throw new ArgumentException("Invalid popup type provided");

            Close();

            GameObject go = Instantiate(POPUPS[data.direction]);
            instance = go.GetComponent<PopupMessageUi>();

            if (!instance)
            {
                Destroy(go);
                throw new ArgumentException("Popup does not have a " + nameof(PopupMessageUi) + " component");
            }
            
            instance.data = data;
            instance.callback = callback;
        }
        
        public static void Close()
        {
            if(instance)
                instance.InstClose();
        }

        public void InstClose()
        {
            Destroy(gameObject);
            Time.timeScale = 1F;

            if (callback != null)
                callback();
        }

        [System.Serializable]
        public struct PopupData
        {
            public PopupDirection direction;

            [Multiline]
            public string text;
            public Sprite image;
            [ColorUsage(false)]
            public Color color;
            public Vector2 expand;
        }

        public enum PopupDirection
        {
            Left,
            Right
        }
        
        #region Init
        private static readonly Dictionary<PopupDirection, GameObject> POPUPS = new Dictionary<PopupDirection, GameObject>();

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            POPUPS.Add(PopupDirection.Left, Resources.Load<GameObject>("Prefabs/UI/Popups/PopupLeftFacing"));
            POPUPS.Add(PopupDirection.Right, Resources.Load<GameObject>("Prefabs/UI/Popups/PopupRightFacing"));
        }
        #endregion
    }
}
