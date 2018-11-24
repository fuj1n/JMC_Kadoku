using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollableArea : MonoBehaviour, IDragHandler
{
    public RectTransform content;
    public Button centerButton;

    private CanvasScaler scaler;

    private void Awake()
    {
        scaler = GetComponentInParent<CanvasScaler>();

        if (centerButton)
            centerButton.onClick.AddListener(() => content.anchoredPosition = Vector2.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
        content.anchoredPosition += eventData.delta / scaler.transform.localScale.x;
        //content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y);
    }
}
