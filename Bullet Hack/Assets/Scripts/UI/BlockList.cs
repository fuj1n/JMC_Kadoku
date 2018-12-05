using UnityEngine;

public class BlockList : MonoBehaviour
{
    public RectTransform anchor;
    public RectTransform root;

    private void Awake()
    {
        foreach (GameObject block in BlockLoader.CreateBlocks())
        {
            CodeBlockDrag drag = block.GetComponentInChildren<CodeBlockDrag>();
            if (!drag)
                continue;

            drag.root = root;
            drag.cloneDrag = true;

            block.transform.SetParent(anchor);
            block.transform.localScale = Vector3.one;
        }
    }
}
