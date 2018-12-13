using UnityEngine;
using UnityEngine.EventSystems;

public class ExpandableExitHide : MonoBehaviour, IPointerExitHandler
{
    private EventSystem system;

    private void Awake()
    {
        system = FindObjectOfType<EventSystem>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        system.SetSelectedGameObject(null, eventData);
    }
}
