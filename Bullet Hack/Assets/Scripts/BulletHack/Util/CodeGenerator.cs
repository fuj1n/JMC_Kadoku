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
        public Vector2Int startPos = new Vector2Int(1, 1);

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

        private void Start()
        {
            Vector2Int pos = startPos;
            
            SerializedBlock rootBlock = new SerializedBlock("repeat.forever");

            SerializedBlock current = Generate(ref pos);
            rootBlock.blockIn = current;

            for (int i = 0; i < Random.Range(5, 20); i++)
            {
                SerializedBlock next = Generate(ref pos);
                current.child = next;
                current = next;
            }

            current.child = GenerateReturnPath(pos);
            
            SerializedBlock.Deserialize(rootBlock, root).GetComponent<CodeBlockDrag>().ConnectTo(manager.outAnchor.parent);
        }

        private SerializedBlock Generate(ref Vector2Int pos)
        {
            if (Random.Range(0, 100) >= 50) return new SerializedBlock("fire");
            SerializedBlock block = new SerializedBlock("move");

            List<MoveAction.Direction> directions = new List<MoveAction.Direction>();
            
            if(pos.x > 0)
                directions.Add(MoveAction.Direction.LEFT);
            if(pos.x < maxPos.x)
                directions.Add(MoveAction.Direction.RIGHT);
            if(pos.y > 0)
                directions.Add(MoveAction.Direction.UP);
            if(pos.y < maxPos.y)
                directions.Add(MoveAction.Direction.DOWN);

            MoveAction.Direction dir = directions[Random.Range(0, directions.Count)];
            
            block.values.Add("direction", (int)dir);

            switch (dir)
            {
                case MoveAction.Direction.UP:
                    pos.y--;
                    break;
                case MoveAction.Direction.DOWN:
                    pos.y++;
                    break;
                case MoveAction.Direction.LEFT:
                    pos.x--;
                    break;
                case MoveAction.Direction.RIGHT:
                    pos.x++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return block;
        }

        private SerializedBlock GenerateReturnPath(Vector2Int pos)
        {
            SerializedBlock current = null;
            
            while (pos != startPos)
            {
                SerializedBlock block = new SerializedBlock("move");

                if (current != null)
                    current.child = block;

                current = block;

                MoveAction.Direction dir = MoveAction.Direction.UP;

                if (pos.x > startPos.x)
                {
                    dir = MoveAction.Direction.LEFT;
                    pos.x--;
                }
                else if (pos.x < startPos.x)
                {
                    dir = MoveAction.Direction.RIGHT;
                    pos.x++;
                }
                else if (pos.y > startPos.y)
                {
                    dir = MoveAction.Direction.UP;
                    pos.y--;
                }
                else if (pos.y < startPos.y)
                {
                    dir = MoveAction.Direction.DOWN;
                    pos.y++;
                }

                block.values.Add("direction", (int) dir);
            }

            return current;
        }
    }
}