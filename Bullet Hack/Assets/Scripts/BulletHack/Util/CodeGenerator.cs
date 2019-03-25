using System;
using System.Collections.Generic;
using BulletHack.Scripting;
using BulletHack.Scripting.Action;
using BulletHack.UI;
using BulletHack.UI.Blocks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BulletHack.Util
{
    public class CodeGenerator : MonoBehaviour
    {
        public Vector2Int gridSize = new Vector2Int(3, 3);
        [NonSerialized]
        public Vector2Int startPos = new Vector2Int(1, 1);
        [NonSerialized]
        public int turnCount = 10;

        private Vector2Int maxPos;

        private BlockManagerBase manager;
        private Transform root;

        private void Awake()
        {
            manager = GetComponent<BlockManagerBase>();
            root = manager.GetComponent<CodeBlockDrag>().root;

            // Position is 0-indexed, so the maxPos is 1 less than the grid size
            maxPos = gridSize - Vector2Int.one;
        }

        public void GenerateCode()
        {
            if (manager.outConnector)
            {
                BlockManager man = manager.outConnector;
                manager.Disconnect(man);
                Destroy(man.gameObject);
            }

            Vector2Int pos = startPos;

            SerializedBlock current = null;
            SerializedBlock rootBlock = null;
            
            for (int i = 1; i < turnCount; i++)
            {
                SerializedBlock next = Generate(ref pos);

                if (current != null)
                    current.child = next;
                else
                    rootBlock = next;   
                    
                current = next;
            }

            SerializedBlock.Deserialize(rootBlock, root).GetComponent<CodeBlockDrag>().ConnectTo(manager.outAnchor.parent);
        }

        private SerializedBlock Generate(ref Vector2Int pos)
        {
            if (Random.Range(0, 100) >= 50) return new SerializedBlock("fire");
            SerializedBlock block = new SerializedBlock("move");

            List<MoveAction.Direction> directions = new List<MoveAction.Direction>();

            if (pos.x > 0)
                directions.Add(MoveAction.Direction.Left);
            if (pos.x < maxPos.x)
                directions.Add(MoveAction.Direction.Right);
            if (pos.y > 0)
                directions.Add(MoveAction.Direction.Up);
            if (pos.y < maxPos.y)
                directions.Add(MoveAction.Direction.Down);

            MoveAction.Direction dir = directions[Random.Range(0, directions.Count)];

            block.values.Add("direction", (int) dir);

            switch (dir)
            {
                case MoveAction.Direction.Up:
                    pos.y--;
                    break;
                case MoveAction.Direction.Down:
                    pos.y++;
                    break;
                case MoveAction.Direction.Left:
                    pos.x--;
                    break;
                case MoveAction.Direction.Right:
                    pos.x++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return block;
        }
    }
}