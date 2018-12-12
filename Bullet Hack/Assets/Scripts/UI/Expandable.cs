using UnityEngine;
using UnityEngine.EventSystems;

public class Expandable : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject content;

    private void Awake()
    {
        content.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        content.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        content.SetActive(false);
    }
}
