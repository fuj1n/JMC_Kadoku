using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollableArea : MonoBehaviour, IDragHandler, IScrollHandler
{
    public RectTransform content;
    public Button centerButton;

    public float zoomSensitivity = 0.25F;

    public Image grid;
    public TextMeshProUGUI coords;

    private RectTransform gridRect;

    private string coordsFormat;

    private void Awake()
    {
        if (centerButton)
            centerButton.onClick.AddListener(() => content.anchoredPosition = Vector2.zero);

        if (grid)
            gridRect = grid.GetComponent<RectTransform>();

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
            float y = content.anchoredPosition.y % (grid.sprite.bounds.size.y * grid.sprite.pixelsPerUnit * gridRect.localScale.y);
            float x = content.anchoredPosition.x % (grid.sprite.bounds.size.x * grid.sprite.pixelsPerUnit * gridRect.localScale.x);

            gridRect.anchoredPosition = new Vector2(x, y);
        }

        UpdateCoords();
    }

    public void OnScroll(PointerEventData eventData)
    {
        content.localScale += Vector3.one * eventData.scrollDelta.y * zoomSensitivity;

        float x = Mathf.Clamp(content.localScale.x, .25F, 4F);
        float y = Mathf.Clamp(content.localScale.y, .25F, 4F);
        float z = Mathf.Clamp(content.localScale.z, .25F, 4F);

        content.localScale = new Vector3(x, y, z);
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
