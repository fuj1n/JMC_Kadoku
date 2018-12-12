using UnityEngine;

public class CodeGenerator : MonoBehaviour
{
    private BlockManagerBase manager;
    private RectTransform root;

    private void Awake()
    {
        manager = GetComponent<BlockManagerBase>();
        root = manager.GetComponent<CodeBlockDrag>().root;
    }

    private void Start()
    {
        CodeBlockDrag repeatBlock = BlockList.Instance.GetBlock("Repeat Forever").GetComponent<CodeBlockDrag>().Clone(root, root);
        repeatBlock.ConnectTo(manager.outAnchor.parent);
    }
}
