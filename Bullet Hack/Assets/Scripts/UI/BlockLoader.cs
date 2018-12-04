using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

            RectTransform vars = blocks[b].transform.Find("Vars")?.GetComponent<RectTransform>();
            if (!vars)
                vars = blocks[b].GetComponent<RectTransform>();

            foreach (FieldInfo input in block.inputs)
            {
                GameObject box = new GameObject(input.Name + " box");
                RectTransform boxRect = box.AddComponent<RectTransform>();
                box.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
                box.AddComponent<VerticalLayoutGroup>();
                boxRect.SetParent(vars);

                GameObject label = new GameObject("Label");
                label.AddComponent<RectTransform>().SetParent(boxRect);
                label.AddComponent<TextMeshProUGUI>().text = input.Name; // TODO: friendly name

                if (input.FieldType.IsEnum)
                {

                }
                else
                {

                }
            }
        }

        return blocks;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Debug.Log("variableNameThatIWantToBeFriendly".ToFriendly());

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
                          select field).ToArray()
            };

            switch (type)
            {
                case BlockAttribute.BlockType.STANDARD:
                    block.template = BLOCK_GENERAL;
                    break;
                case BlockAttribute.BlockType.BRACKET:
                    block.template = BLOCK_BRACKET;
                    break;
            }

            RectTransform rect = block.template.GetComponent<RectTransform>();

            loadedBlocks.Add(block);
        }

        blocks = loadedBlocks.ToArray();
    }

    private struct Block
    {
        public GameObject template;
        public System.Type component;
        public FieldInfo[] inputs;
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
