using BulletHack.Scripting;
using BulletHack.UI;
using BulletHack.UI.BlockManager;
using UnityEngine;

namespace BulletHack.Util
{
    public class CodeGenerator : MonoBehaviour
    {
        private BlockManagerBase manager;
        private Transform root;

        private void Awake()
        {
            manager = GetComponent<BlockManagerBase>();
            root = manager.GetComponent<CodeBlockDrag>().root;
        }

        private void Start()
        {
            SerializedBlock rootBlock = new SerializedBlock("repeat.forever");

            SerializedBlock current = Generate();
            rootBlock.blockIn = current;

            for (int i = 0; i < Random.Range(5, 20); i++)
            {
                SerializedBlock next = Generate();
                current.child = next;
                current = next;
            }


            SerializedBlock.Deserialize(rootBlock, root).GetComponent<CodeBlockDrag>().ConnectTo(manager.outAnchor.parent);
        }

        private static SerializedBlock Generate()
        {
            if (Random.Range(0, 100) >= 50) return new SerializedBlock("fire");
            SerializedBlock block = new SerializedBlock("move");

            int direction = Random.Range(0, 100) < 80 ? Random.Range(2, 4) : Random.Range(0, 2);

            block.values.Add("direction", direction);

            return block;
        }

        //void Test()
        //{
        //    SerializedBlock.Deserialize("{\"name\": \"Move\",\"values\": { \"direction\": 3},\"child\":{\"name\": \"Move\",\"values\":{\"direction\": 2}}}", root).GetComponent<CodeBlockDrag>().ConnectTo(manager.outAnchor.parent);
        //}
    }
}