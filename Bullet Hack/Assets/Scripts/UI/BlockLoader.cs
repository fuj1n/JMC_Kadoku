using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

public static class BlockLoader
{
    public const string TEMPLATE_PATH = "Prefabs/UI/Blocks/";
    public static readonly GameObject BLOCK_GENERAL = Resources.Load<GameObject>(TEMPLATE_PATH + "GeneralBlock");
    public static readonly GameObject BLOCK_BRACKET = Resources.Load<GameObject>(TEMPLATE_PATH + "BracketBlock");

    private static Block[] blocks;

    public static GameObject[] CreateBlocks()
    {
        GameObject[] blocks = new GameObject[BlockLoader.blocks.Length];

        for (int b = 0; b < blocks.Length; b++)
        {
            Block block = BlockLoader.blocks[b];

            blocks[b] = Object.Instantiate(block.template);
            ActionBase action = (ActionBase)blocks[b].AddComponent(block.component);
            action.nameText = blocks[b].GetComponentInChildren<TextMeshProUGUI>();

            RectTransform vars = blocks[b].transform.Find("Vars")?.GetComponent<RectTransform>();
            if (!vars)
                vars = blocks[b].GetComponent<RectTransform>();

            foreach (FieldInfo input in block.inputs)
            {
                bool reverse = block.reversed.Contains(input);

                GameObject box = new GameObject(input.Name + " box");
                RectTransform boxRect = box.AddComponent<RectTransform>();
                boxRect.SetParent(vars, true);

                boxRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0F, vars.sizeDelta.y);
                boxRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0F, 160F);
                boxRect.localScale = Vector3.one;

                GameObject label = new GameObject("Label");

                RectTransform labelRect = label.AddComponent<RectTransform>();
                labelRect.SetParent(boxRect, true);
                labelRect.localScale = Vector3.one;

                labelRect.anchorMin = new Vector2(0, 1);
                labelRect.anchorMax = new Vector2(1, 1);

                TextMeshProUGUI lblText = label.AddComponent<TextMeshProUGUI>();
                lblText.text = input.Name.ToFriendly(true);
                lblText.alignment = TextAlignmentOptions.MidlineLeft;
                lblText.enableAutoSizing = true;
                lblText.fontSizeMin = 0F;
                lblText.fontSizeMax = 24F;

                labelRect.offsetMin = Vector2.zero;
                labelRect.offsetMax = Vector2.zero;
                labelRect.SetInsetAndSizeFromParentEdge(reverse ? RectTransform.Edge.Bottom : RectTransform.Edge.Top, 0F, boxRect.sizeDelta.y * .4F);

                RectTransform vRect;
                ValueBinder binder;

                if (input.FieldType.IsEnum)
                {
                    GameObject dropdown = Object.Instantiate(CommonResources.DROPDOWN, boxRect);

                    binder = dropdown.AddComponent<EnumBinder>();

                    vRect = dropdown.GetComponent<RectTransform>();
                }
                else if (input.FieldType == typeof(bool))
                {
                    GameObject toggle = Object.Instantiate(CommonResources.TOGGLE, boxRect);

                    binder = toggle.AddComponent<BoolBinder>();

                    vRect = toggle.GetComponent<RectTransform>();
                }
                else
                {
                    GameObject ifield = Object.Instantiate(CommonResources.INPUT_FIELD, boxRect);

                    binder = ifield.AddComponent<ValueBinder>();

                    vRect = ifield.GetComponent<RectTransform>();
                }

                binder.field = input;
                binder.obj = action;

                vRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0F, boxRect.sizeDelta.x);
                vRect.SetInsetAndSizeFromParentEdge(reverse ? RectTransform.Edge.Top : RectTransform.Edge.Bottom, 0F, boxRect.sizeDelta.y * .6F);
            }

            Canvas.ForceUpdateCanvases();
            RectTransform blockRect = blocks[b].GetComponent<RectTransform>();
            blockRect.sizeDelta = new Vector2(blockRect.sizeDelta.x + vars.sizeDelta.x, blockRect.sizeDelta.y);

            blockRect.localScale = Vector3.one;
        }

        return blocks;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // Find all the classes that have the BlockAttribute attribute
        var discoveredBlocks = from assembly in System.AppDomain.CurrentDomain.GetAssemblies()
                               from t in assembly.GetTypes()
                               where typeof(ActionBase).IsAssignableFrom(t)
                               let attrib = t.GetCustomAttribute<BlockAttribute>(false)
                               where attrib != null
                               select new { type = t, attribute = attrib };

        List<Block> loadedBlocks = new List<Block>();

        foreach (var found in discoveredBlocks)
        {
            BlockAttribute.BlockType type = found.attribute.blockType;

            Block block = new Block
            {
                component = found.type,
                inputs = (from field in found.type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                          where field.GetCustomAttribute<ActionBase.InputVarAttribute>(true) != null
                          select field).ToArray(),
            };

            block.reversed = (from field in block.inputs
                              where field.GetCustomAttribute<ActionBase.InputVarAttribute>(true).Reverse
                              select field).ToList();

            switch (type)
            {
                case BlockAttribute.BlockType.STANDARD:
                    block.template = BLOCK_GENERAL;
                    break;
                case BlockAttribute.BlockType.BRACKET:
                    block.template = BLOCK_BRACKET;
                    break;
            }

            loadedBlocks.Add(block);
        }

        blocks = loadedBlocks.ToArray();
    }

    private struct Block
    {
        public GameObject template;
        public System.Type component;
        public FieldInfo[] inputs;
        public List<FieldInfo> reversed;
    }

    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class BlockAttribute : System.Attribute
    {
        public BlockType blockType;

        public BlockAttribute(BlockType blockType)
        {
            this.blockType = blockType;
        }

        public enum BlockType
        {
            STANDARD,
            BRACKET
        }
    }
}
