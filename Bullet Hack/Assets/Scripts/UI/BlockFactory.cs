using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BlockFactory {
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
    /// <param name="width">The block width</param>
    /// <param name="height">The block height</param>
    /// <param name="types">The type for each corner, providing one will make all 4 that value and providing 2 will stagger them</param>
    /// <returns>The game object that contains the given block</returns>
    public static GameObject CreateBlock(RectTransform parent, Vector2 pos, Vector2 size, params int[] types)
    {
        return CreateBlock(parent, Vector2.one * 0.5F, Vector2.one * 0.5F, pos, size, types);
    }

    /// <summary>
    /// Creates a UI block with the given parameters and returns its instance
    /// </summary>
    /// <param name="parent">The RectTransform that contains this box</param>
    /// <param name="anchorMin">The min bound of the anchor point</param>
    /// <param name="anchorMax">The max bound of the anchor point</param>
    /// <param name="x">The block X position</param>
    /// <param name="y">The block Y position</param>
    /// <param name="width">The block width</param>
    /// <param name="height">The block height</param>
    /// <param name="types">The type for each corner, providing one will make all 4 that value and providing 2 will stagger them</param>
    /// <returns>The game object that contains the given block</returns>
    public static GameObject CreateBlock(RectTransform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pos, Vector2 size, params int[] types)
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
        else if (types.Length != 4)
            Array.Resize(ref types, 4);

        GameObject go = new GameObject("Block");

        go.transform.SetParent(parent);

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = pos;

        // TODO implement block creation

        return go;
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
                sideTop = blockSprites["t"],
                sideBottom = blockSprites["b"],
                sideLeft = blockSprites["l"],
                sideRight = blockSprites["r"],

                cornerTopLeft = blockSprites["tl"],
                cornerTopRight = blockSprites["tr"],
                cornerBottomLeft = blockSprites["bl"],
                cornerBottomRight = blockSprites["br"],

                fill = blockSprites["f"]
            });
        }
    }

    private struct Block
    {
        public Sprite sideTop;
        public Sprite sideBottom;
        public Sprite sideLeft;
        public Sprite sideRight;

        public Sprite cornerTopLeft;
        public Sprite cornerTopRight;
        public Sprite cornerBottomLeft;
        public Sprite cornerBottomRight;

        public Sprite fill;
    }
}
