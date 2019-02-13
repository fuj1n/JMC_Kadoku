using UnityEngine;
using UnityEngine.EventSystems;

namespace BulletHack.UI
{
    public class ExpandableExitHide : MonoBehaviour, IPointerExitHandler
    {
        private EventSystem system;

        private void Awake()
        {
            system = FindObjectOfType<EventSystem>();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            system.SetSelectedGameObject(null, eventData);
        }
    }
}