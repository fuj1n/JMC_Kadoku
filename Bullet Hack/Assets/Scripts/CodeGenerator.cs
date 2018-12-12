using UnityEngine;

public class CodeGenerator : MonoBehaviour
{
    private BlockManagerBase manager;

    private void Awake()
    {
        manager = GetComponent<BlockManagerBase>();
    }

    private void Start()
    {
        GameObject repeatBlockTemplate = BlockList.Instance.GetBlock("Repeat Forever");
        GameObject repeatBlock = Instantiate(repeatBlockTemplate);
        repeatBlock.GetComponent<CodeBlockDrag>().UpdateBinders(repeatBlockTemplate.GetComponent<CodeBlockDrag>().binders);

        BlockManager repeatManager = repeatBlock.GetComponent<BlockManager>();
        RectTransform anchor = manager.Connect(manager.outAnchor.parent, repeatManager);

        repeatManager.transform.SetParent(manager.outAnchor);

        RectTransform rect = repeatManager.GetComponent<RectTransform>();

        rect.localScale = Vector3.one;

        rect.position += anchor.position - repeatManager.inAnchor.position;
    }
}
