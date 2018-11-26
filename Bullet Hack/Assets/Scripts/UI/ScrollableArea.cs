using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ScrollableArea : MonoBehaviour, IDragHandler, IScrollHandler
{
    public RectTransform content;
    public Button centerButton;

    public float zoomSensitivity = 0.1F;
    public RectTransform zoomAnchor;

    public Image grid;
    public TextMeshProUGUI coords;

    private RectTransform gridRect;

    private string coordsFormat;

    private float zoomAmount = 1F;

    private void Awake()
    {
        zoomAnchor.localScale = Vector3.one * zoomAmount;

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
        zoomAmount = Mathf.Clamp(zoomAmount + eventData.scrollDelta.y * zoomSensitivity, .25F, 4F);

        zoomAnchor.DOScale(zoomAmount, .25F);
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
