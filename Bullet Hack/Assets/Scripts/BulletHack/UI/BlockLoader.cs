using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BulletHack.Scripting.Action;
using BulletHack.Scripting.Action.BlockAction;
using BulletHack.UI.Binder;
using BulletHack.Util;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BulletHack.UI
{
    public static class BlockLoader
    {
        public const string TEMPLATE_PATH = "Prefabs/UI/Blocks/";
        public static readonly GameObject BLOCK_GENERAL = Resources.Load<GameObject>(TEMPLATE_PATH + "GeneralBlock");
        public static readonly GameObject BLOCK_BRACKET = Resources.Load<GameObject>(TEMPLATE_PATH + "BracketBlock");

        private static Dictionary<string, Block> blocks = new Dictionary<string, Block>();

        public static GameObject CreateBlock(string id, Transform root = null)
        {
            if(!blocks.ContainsKey(id))
                throw new ArgumentException("Block " + id + " does not exist or is not yet loaded.");
            
            Block block = blocks[id];

            GameObject blockObj = Object.Instantiate(block.template, root);
            ActionBase action = (ActionBase) blockObj.AddComponent(block.component);
            action.nameText = blockObj.GetComponentInChildren<TextMeshProUGUI>();
            action.SetId(block.id);

            RectTransform varsContainer = blockObj.transform.Find("Vars")?.GetComponent<RectTransform>();
            if (!varsContainer)
                varsContainer = blockObj.GetComponent<RectTransform>();

            foreach (FieldInfo input in block.inputs)
            {
                bool reverse = block.reversed.Contains(input);
                
                    GameObject box = new GameObject(input.Name + " box");
                    RectTransform boxRect = box.AddComponent<RectTransform>();
                    boxRect.SetParent(varsContainer, true);

                    boxRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0F, varsContainer.sizeDelta.y);
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
            RectTransform blockRect = blockObj.GetComponent<RectTransform>();
            blockRect.sizeDelta = new Vector2(blockRect.sizeDelta.x + varsContainer.sizeDelta.x, blockRect.sizeDelta.y);

            blockRect.localScale = Vector3.one;

            CodeBlockDrag drag = blockObj.GetComponent<CodeBlockDrag>();
            
            drag.root = root;
            
            return blockObj;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // Find all the classes that have the BlockAttribute attribute
            var discoveredBlocks = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from t in assembly.GetTypes()
                    where typeof(ActionBase).IsAssignableFrom(t)
                    let attrib = t.GetCustomAttribute<BlockAttribute>(false)
                    where attrib != null
                    select new {type = t, attribute = attrib};

            List<Block> loadedBlocks = new List<Block>();

            foreach (var found in discoveredBlocks)
            {
                bool isBracketBlock = typeof(ActionBlockBase).IsAssignableFrom(found.type);

                Block block = new Block
                {
                        id = found.attribute.id,
                        component = found.type,
                        inputs = (from field in found.type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                where field.GetCustomAttribute<ActionBase.InputVarAttribute>(true) != null
                                select field).ToArray()
                };

                block.reversed = (from field in block.inputs
                        where field.GetCustomAttribute<ActionBase.InputVarAttribute>(true).Reverse
                        select field).ToList();

                block.template = isBracketBlock ? BLOCK_BRACKET : BLOCK_GENERAL;

                loadedBlocks.Add(block);
            }

            blocks = loadedBlocks.ToDictionary(x => x.id);
        }

        private struct Block
        {
            public string id;
            
            public GameObject template;
            public Type component;
            public FieldInfo[] inputs;
            public List<FieldInfo> reversed;
        }

        [System.AttributeUsage(AttributeTargets.Class, Inherited = false)]
        public sealed class BlockAttribute : Attribute
        {
            public readonly string id;
            
            public BlockAttribute(string id)
            {
                this.id = id;
            }
        }
    }
}