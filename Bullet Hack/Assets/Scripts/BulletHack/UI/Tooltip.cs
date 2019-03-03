using UnityEngine;
using UnityEngine.EventSystems;

namespace BulletHack.UI
{
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string title;
        [Multiline]
        public string description;

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipRenderer.SetTooltip(title, description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipRenderer.CloseTooltip();
        }
    }
}
