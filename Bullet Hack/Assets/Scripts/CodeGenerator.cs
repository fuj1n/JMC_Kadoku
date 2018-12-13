using UnityEngine;

public class CodeGenerator : MonoBehaviour
{
    private BlockManagerBase manager;
    private RectTransform root;

    private void Awake()
    {
        manager = GetComponent<BlockManagerBase>();
        root = manager.GetComponent<CodeBlockDrag>().root;
    }

    private void Start()
    {
        SerializedBlock rootBlock = new SerializedBlock("Repeat Forever");

        SerializedBlock current = Generate();
        rootBlock.blockIn = current;

        for(int i = 0; i < Random.Range(5, 20); i++)
        {
            SerializedBlock next = Generate();
            current.child = next;
            current = next;
        }



        SerializedBlock.Deserialize(rootBlock, root).GetComponent<CodeBlockDrag>().ConnectTo(manager.outAnchor.parent);
    }

    private SerializedBlock Generate()
    {
        if(Random.Range(0, 100) < 50)
        {
            SerializedBlock block = new SerializedBlock("Move");

            int direction;

            if (Random.Range(0, 100) < 80)
                direction = Random.Range(2, 4);
            else
                direction = Random.Range(0, 2);

            block.values.Add("direction", direction);

            return block;
        }
        else
        {
            return new SerializedBlock("Fire");
        }
    }

    //void Test()
    //{
    //    SerializedBlock.Deserialize("{\"name\": \"Move\",\"values\": { \"direction\": 3},\"child\":{\"name\": \"Move\",\"values\":{\"direction\": 2}}}", root).GetComponent<CodeBlockDrag>().ConnectTo(manager.outAnchor.parent);
    //}
}
