using UnityEngine;

public class BlockConnectorIn : MonoBehaviour
{
    private CodeBlockDrag parent;

    private Transform anchor;

    private void Awake()
    {
        parent = GetComponentInParent<CodeBlockDrag>();

        anchor = transform.Find("Anchor");
        if (!anchor)
            anchor = transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOutputBlock(collision))
            return;

        parent.SetTargetBlock(anchor, collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsOutputBlock(collision))
            return;

        parent.SetTargetBlock(anchor, null);
    }

    public bool IsOutputBlock(Collider2D collision)
    {
        return collision.CompareTag("Block_OUT");
    }
}
