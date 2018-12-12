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
        Invoke("Test", 1F);

        //CodeBlockDrag repeatBlock = BlockList.Instance.GetBlock("Repeat Forever").GetComponent<CodeBlockDrag>().Clone(root, root);
        //repeatBlock.ConnectTo(manager.outAnchor.parent);
    }

    void Test()
    {
        SerializedBlock.Deserialize("{\"name\": \"Move\",\"values\": { \"direction\": 3},\"child\":{\"name\": \"Move\",\"values\":{\"direction\": 2}}}", root).GetComponent<CodeBlockDrag>().ConnectTo(manager.outAnchor.parent);
    }
}
