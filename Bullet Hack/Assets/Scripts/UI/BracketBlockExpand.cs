using UnityEngine;

public class BracketBlockExpand : MonoBehaviour
{
    public RectTransform expandPoint;

    public float inputHeight = 32F;

    private RectTransform rect;

    private float minSize;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        minSize = rect.sizeDelta.y;
    }

    private void Update()
    {
        float size = CountChildSizes(expandPoint);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, minSize + size);
    }

    public float CountChildSizes(RectTransform transform)
    {
        float size = transform.CompareTag("UI-Block") ? transform.rect.size.y - inputHeight : 0F;
        foreach (Transform t in transform)
        {
            RectTransform rect = t.GetComponent<RectTransform>();
            if (rect)
                size += CountChildSizes(rect);
        }

        return size;
    }
}
