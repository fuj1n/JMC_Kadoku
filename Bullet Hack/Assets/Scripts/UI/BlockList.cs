using System.Collections.Generic;
using UnityEngine;

public class BlockList : MonoBehaviour
{
    public static BlockList Instance { get; private set; }

    public RectTransform anchor;
    public RectTransform root;

    private Dictionary<string, GameObject> blocks = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;

        foreach (GameObject block in BlockLoader.CreateBlocks())
        {
            CodeBlockDrag drag = block.GetComponentInChildren<CodeBlockDrag>();
            if (!drag)
                continue;

            drag.root = root;
            drag.cloneDrag = true;

            block.transform.SetParent(anchor);
            block.transform.localScale = Vector3.one;

            blocks.Add(block.GetComponent<ActionBase>().GetName(), block);
        }
    }

    public GameObject GetBlock(string block)
    {
        if (!blocks.ContainsKey(block))
            return null;
        return blocks[block];
    }

    //public GameObject LoadFromString(string s)
    //{

    //}
}
