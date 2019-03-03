using TMPro;
using UnityEngine;

namespace BulletHack.UI
{
    public class TooltipRenderer : MonoBehaviour
    {
        private static TooltipRenderer instance;
        
        public GameObject tooltipTemplate;

        private string tooltipFormat;
        private TextMeshProUGUI tooltip;
        private RectTransform rect;

        private Canvas canvas;
        
        private void Awake()
        {
            instance = this;
            canvas = GetComponentInParent<Canvas>();
            
            
            GameObject frame = Instantiate(tooltipTemplate, transform);

            rect = frame.GetComponent<RectTransform>();
            if (!rect)
                rect = frame.AddComponent<RectTransform>();

            tooltip = GetComponentInChildren<TextMeshProUGUI>();
            tooltipFormat = tooltip.text;
            
            rect.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            rect.position = Input.mousePosition - (Vector3.right + Vector3.down) * 5F;

            Rect window = ((RectTransform) transform).rect;

            Vector2 pos = rect.anchoredPosition;
            
            if (-rect.offsetMin.y > window.height)
                pos.y = -window.height + rect.sizeDelta.y;
            else if (-rect.offsetMax.y < 0F)
                pos.y = 0F;
            if (rect.offsetMin.x < 0F)
                pos.x = 0F;
            else if (rect.offsetMax.x > window.width)
                pos.x = window.width - rect.sizeDelta.x;
            
            rect.anchoredPosition = pos;
        }

        public static void SetTooltip(string name, string description)
        {
            instance._SetTooltip(name, description);
        }

        public static void CloseTooltip()
        {
            instance.rect.gameObject.SetActive(false);
        }

        private void _SetTooltip(string name, string description)
        {
            tooltip.text = string.Format(tooltipFormat, name, description).Trim(' ', '\n', '\r');
            rect.gameObject.SetActive(true);
            tooltip.ForceMeshUpdate();
            
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, tooltip.preferredHeight + 24F);
            tooltip.ForceMeshUpdate();
        }
    }
}
