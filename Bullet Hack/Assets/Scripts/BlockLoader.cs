using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockLoader {
    public const string TEMPLATE_PATH = "Prefabs/UI/Blocks/";
    public static readonly GameObject BLOCK_GENERAL = Resources.Load<GameObject>(TEMPLATE_PATH + "GeneralBlock");
    public static readonly GameObject BLOCK_BRACKET = Resources.Load<GameObject>(TEMPLATE_PATH + "BracketBlock");

    private static Block[] blocks;

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

            Block block = new Block();
            block.component = found.type;

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

            block.inputs = (from field in found.type.GetFields()
                            where field.GetCustomAttribute<ActionBase.InputVarAttribute>(true) != null
                            select field).ToArray();

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
    public sealed class BlockAttribute : System.Attribute {
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
