using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class BlockFactory
{
    private const int BLOCKS_COUNT = 2;
    private const string BLOCKS_RESOURCE = "Sprites/";
    private const string BLOCKS_NAME = "block";

    private static readonly List<Block> blocks = new List<Block>();

    /// <summary>
    /// Creates a UI block with the given parameters and returns its instance
    /// </summary>
    /// <param name="parent">The RectTransform that contains this box</param>
    /// <param name="pos">The position of the block</param>
    /// <param name="size">The size of the block</param>
    /// <param name="color">The color multiplier of the block</param>
    /// <param name="fillType">The type of the fill</param>
    /// <param name="types">The type for each corner, providing one will make all 4 that value and providing 2 will stagger them</param>
    /// <returns>The game object that contains the given block</returns>
    public static GameObject CreateBlock(RectTransform parent, Vector2 pos, Vector2 size, Color color, int fillType, params int[] types)
    {
        return CreateBlock(parent, Vector2.one * 0.5F, Vector2.one * 0.5F, pos, size, color, fillType, types);
    }

    /// <summary>
    /// Creates a UI block with the given parameters and returns its instance
    /// </summary>
    /// <param name="parent">The RectTransform that contains this box</param>
    /// <param name="anchorMin">The min bound of the anchor point</param>
    /// <param name="anchorMax">The max bound of the anchor point</param>
    /// <param name="pos">The position of the block</param>
    /// <param name="size">The size of the block</param>
    /// <param name="color">The color multiplier of the block</param>
    /// <param name="fillType">The type of the fill</param>
    /// <param name="types">The type for each corner, providing one will make all 4 that value and providing 2 will stagger them</param>
    /// <returns>The game object that contains the given block</returns>
    public static GameObject CreateBlock(RectTransform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pos, Vector2 size, Color color, int fillType, params int[] types)
    {
        return CreateBlock(parent, anchorMin, anchorMax, pos, size, color, null, null, fillType, types);
    }

    public static GameObject CreateBlock(RectTransform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pos, Vector2 size, Color color, Vector2[] inConnectors, Vector2[] outConnectors, int fillType, params int[] types)
    {
        // Ensure the length of types is 4, and resize it using various strategies based on length
        if (types.Length != 4)
        {
            if (types == null || types.Length == 0)
                types = Enumerable.Repeat(0, 4).ToArray();
            else if (types.Length == 1)
                types = Enumerable.Repeat(types[0], 4).ToArray();
            else if (types.Length == 2)
            {
                Array.Resize(ref types, 4);
                types[2] = types[0];
                types[3] = types[1];
            }
            else
                Array.Resize(ref types, 4);
        }

        if (fillType >= blocks.Count)
            fillType = 0;

        // Ensure types do not go out of bounds
        for (int i = 0; i < types.Length; i++)
            if (types[i] >= blocks.Count)
                types[i] = 0;

        GameObject block = new GameObject("Block");
        block.transform.SetParent(parent);

        RectTransform blockTransform = block.AddComponent<RectTransform>();
        blockTransform.anchorMin = anchorMin;
        blockTransform.anchorMax = anchorMax;
        blockTransform.anchoredPosition = pos;

        GameObject fillGo = new GameObject("Fill");
        fillGo.transform.SetParent(blockTransform);

        RectTransform fill = fillGo.AddComponent<RectTransform>();
        fill.anchorMin = Vector2.zero;
        fill.anchorMax = Vector2.one;

        // Corners
        for (int i = 0; i < 4; i++)
        {
            Sprite spr = blocks[types[i]].corners[i];

            GameObject go = new GameObject("Corner" + i);
            go.transform.SetParent(blockTransform);

            RectTransform rect = go.AddComponent<RectTransform>();
            rect.sizeDelta = spr.bounds.size * spr.pixelsPerUnit;

            switch (i)
            {
                case 0:
                    rect.pivot = new Vector2(0F, 1F);
                    rect.anchorMin = new Vector2(0F, 1F);
                    rect.anchorMax = new Vector2(0F, 1F);
                    break;
                case 1:
                    rect.pivot = new Vector2(1F, 1F);
                    rect.anchorMin = new Vector2(1F, 1F);
                    rect.anchorMax = new Vector2(1F, 1F);

                    fill.offsetMax = -spr.bounds.size * spr.pixelsPerUnit;
                    break;
                case 2:
                    rect.pivot = new Vector2(0F, 0F);
                    rect.anchorMin = new Vector2(0F, 0F);
                    rect.anchorMax = new Vector2(0F, 0F);

                    fill.offsetMin = spr.bounds.size * spr.pixelsPerUnit;
                    break;
                case 3:
                    rect.pivot = new Vector2(1F, 0F);
                    rect.anchorMin = new Vector2(1F, 0F);
                    rect.anchorMax = new Vector2(1F, 0F);
                    break;
            }

            rect.anchoredPosition = Vector2.zero;

            Image img = go.AddComponent<Image>();
            img.sprite = spr;
            img.color = color;
        }

        // Sides
        for (int i = 0; i < 4; i++)
        {
            Sprite spr = blocks[types[i]].sides[i];
            Sprite corner = blocks[types[i]].corners[i];

            GameObject go = new GameObject("Side" + i);
            go.transform.SetParent(blockTransform);

            RectTransform rect = go.AddComponent<RectTransform>();
            rect.sizeDelta = spr.bounds.size * spr.pixelsPerUnit;

            switch (i)
            {
                case 0:
                    rect.pivot = new Vector2(.5F, 1F);
                    rect.anchorMin = new Vector2(0F, 1F);
                    rect.anchorMax = new Vector2(1F, 1F);

                    rect.anchoredPosition = Vector2.zero;

                    rect.offsetMin = new Vector2(corner.bounds.size.x * corner.pixelsPerUnit, rect.offsetMin.y);
                    rect.offsetMax = new Vector2(-corner.bounds.size.x * corner.pixelsPerUnit, rect.offsetMax.y);
                    break;
                case 1:
                    rect.pivot = new Vector2(.5F, 0F);
                    rect.anchorMin = new Vector2(0F, 0F);
                    rect.anchorMax = new Vector2(1F, 0F);

                    rect.anchoredPosition = Vector2.zero;

                    rect.offsetMin = new Vector2(corner.bounds.size.x * corner.pixelsPerUnit, rect.offsetMin.y);
                    rect.offsetMax = new Vector2(-corner.bounds.size.x * corner.pixelsPerUnit, rect.offsetMax.y);
                    break;
                case 2:
                    rect.pivot = new Vector2(0F, .5F);
                    rect.anchorMin = new Vector2(0F, 0F);
                    rect.anchorMax = new Vector2(0F, 1F);

                    rect.anchoredPosition = Vector2.zero;

                    rect.offsetMin = new Vector2(rect.offsetMin.x, corner.bounds.size.y * corner.pixelsPerUnit);
                    rect.offsetMax = new Vector2(rect.offsetMax.x, -corner.bounds.size.y * corner.pixelsPerUnit);
                    break;
                case 3:
                    rect.pivot = new Vector2(1F, .5F);
                    rect.anchorMin = new Vector2(1F, 0F);
                    rect.anchorMax = new Vector2(1F, 1F);

                    rect.anchoredPosition = Vector2.zero;

                    rect.offsetMin = new Vector2(rect.offsetMin.x, corner.bounds.size.y * corner.pixelsPerUnit);
                    rect.offsetMax = new Vector2(rect.offsetMax.x, -corner.bounds.size.y * corner.pixelsPerUnit);
                    break;
            }

            Image img = go.AddComponent<Image>();
            img.sprite = spr;
            img.color = color;
        }

        Image fillImage = fillGo.AddComponent<Image>();
        fillImage.sprite = blocks[fillType].fill;
        fillImage.color = color;

        return block;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        Dictionary<string, Sprite> blocksSprites = Resources.LoadAll<Sprite>(BLOCKS_RESOURCE).Where(x => x.name.StartsWith(BLOCKS_NAME)).ToDictionary(x => x.name.Substring(x.name.IndexOf('_') + 1));

        for (int i = 0; i < BLOCKS_COUNT; i++)
        {
            Dictionary<string, Sprite> blockSprites = blocksSprites.Where(x => x.Key.StartsWith(i.ToString())).Select(x => x.Value).ToDictionary(x => x.name.Substring(x.name.LastIndexOf('_') + 1));

            blocks.Add(new Block()
            {
                sides = new Sprite[]
                {
                    blockSprites["t"],
                    blockSprites["b"],
                    blockSprites["l"],
                    blockSprites["r"]
                },

                corners = new Sprite[]
                {
                    blockSprites["tl"],
                    blockSprites["tr"],
                    blockSprites["bl"],
                    blockSprites["br"]
                },

                fill = blockSprites["f"]
            });
        }

        CreateBlock(UnityEngine.Object.FindObjectOfType<Canvas>().GetComponent<RectTransform>(), Vector2.one * 32F, Vector2.one * 128F, Color.red, 0, 1, 0, 0, 1);
    }

    private struct Block
    {
        public Sprite[] sides;
        public Sprite[] corners;
        public Sprite fill;
    }
}
