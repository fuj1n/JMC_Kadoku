using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollableArea : MonoBehaviour, IDragHandler, IScrollHandler
{
    public RectTransform content;
    public Button centerButton;

    public float zoomSensitivity = 0.25F;
    public RectTransform zoomAnchor;

    public Image grid;
    public TextMeshProUGUI coords;

    private RectTransform gridRect;

    private string coordsFormat;

    private void Awake()
    {
        if (centerButton)
            centerButton.onClick.AddListener(() => content.anchoredPosition = Vector2.zero);

        if (grid)
        {
            gridRect = grid.GetComponent<RectTransform>();

            gridRect.offsetMin = -grid.sprite.rect.size;
            gridRect.offsetMax = grid.sprite.rect.size;
        }

        if (coords)
        {
            coordsFormat = coords.text;
            UpdateCoords();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        content.anchoredPosition += eventData.delta / content.parent.lossyScale;

        if (grid)
        {
            float x = (content.anchoredPosition.x * zoomAnchor.localScale.x) % (grid.sprite.rect.size.x * gridRect.localScale.x);
            float y = (content.anchoredPosition.y * zoomAnchor.localScale.x) % (grid.sprite.rect.size.y * gridRect.localScale.y);

            gridRect.anchoredPosition = new Vector2(x, y);
        }

        UpdateCoords();
    }

    public void OnScroll(PointerEventData eventData)
    {
        Vector3 zoom = zoomAnchor.localScale + Vector3.one * eventData.scrollDelta.y * zoomSensitivity;

        zoom.x = Mathf.Clamp(zoom.x, .25F, 4F);
        zoom.y = Mathf.Clamp(zoom.y, .25F, 4F);
        zoom.z = Mathf.Clamp(zoom.z, .25F, 4F);

        zoomAnchor.localScale = zoom;
    }

    private void UpdateCoords()
    {
        if (!coords)
            return;

        string x = content.anchoredPosition.x.ToString("n1");
        string y = content.anchoredPosition.y.ToString("n1");

        coords.text = string.Format(coordsFormat, x, y);
    }
}
