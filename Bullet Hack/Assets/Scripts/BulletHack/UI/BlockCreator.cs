using BulletHack.UI.Blocks;
using UnityEngine;

namespace BulletHack.UI
{
    public class BlockCreator : MonoBehaviour
    {
        public Transform root;
        public BlockManagerBase startBlock;

        public void Create(string id)
        {
            CodeBlockDrag drag = BlockLoader.CreateBlock(id, root).GetComponent<CodeBlockDrag>();

            if (startBlock)
            {
                BlockManagerBase end = startBlock;
                Transform anchor = startBlock.outAnchor.parent;

                while (true)
                {
                    if (!end.outConnector)
                    {
                        if (end is BracketBlockManager endBm)
                        {
                            if (endBm.bracketConnector)
                            {
                                end = endBm.bracketConnector;
                                anchor = end.outAnchor.parent;
                                continue;
                            }

                            anchor = endBm.bracketAnchor.parent;
                        }

                        break;
                    }

                    end = end.outConnector;
                    anchor = end.outAnchor.parent;
                }

                drag.ConnectTo(anchor);
            }
        }
    }
}