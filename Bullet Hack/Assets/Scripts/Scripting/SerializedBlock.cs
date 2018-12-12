using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class SerializedBlock
{
    public string name;
    public Dictionary<string, object> values = new Dictionary<string, object>();
    public SerializedBlock child;

    public SerializedBlock blockIn;

    public static string Serialize(BlockManagerBase manager)
    {
        return "";
    }

    public static BlockManagerBase Deserialize(string s, RectTransform root)
    {
        SerializedBlock block = JsonConvert.DeserializeObject<SerializedBlock>(s);

        return Deserialize(block, root);
    }

    public static BlockManagerBase Deserialize(SerializedBlock block, RectTransform root)
    {
        BlockList list = BlockList.Instance;

        CodeBlockDrag drag = list.GetBlock(block.name).GetComponent<CodeBlockDrag>().Clone(root, root);
        BlockManagerBase manager = drag.GetComponent<BlockManagerBase>();

        foreach (ValueBinder binder in drag.GetComponentsInChildren<ValueBinder>())
        {
            if (block.values.ContainsKey(binder.field.Name))
            {
                object value = block.values[binder.field.Name];

                if (value is long && ((long)value) >= int.MinValue && ((long)value) <= int.MaxValue)
                    value = System.Convert.ToInt32(value);

                if (binder.field.FieldType.IsEnum)
                    binder.field.SetValue(binder.obj, System.Enum.ToObject(binder.field.FieldType, value));
                else
                    binder.field.SetValue(binder.obj, System.Convert.ChangeType(value, binder.field.FieldType));
            }
        }

        if (manager is BracketBlockManager && block.blockIn != null)
            Deserialize(list, block.blockIn, ((BracketBlockManager)manager).bracketAnchor.parent, root);

        if (block.child != null)
            Deserialize(list, block.child, manager.outAnchor.parent, root);

        return manager;
    }

    private static void Deserialize(BlockList list, SerializedBlock block, Transform outConnector, RectTransform root)
    {
        CodeBlockDrag drag = list.GetBlock(block.name).GetComponent<CodeBlockDrag>().Clone(root, root);
        drag.ConnectTo(outConnector);

        BlockManagerBase manager = drag.GetComponent<BlockManagerBase>();

        if (manager is BracketBlockManager && block.blockIn != null)
            Deserialize(list, block.blockIn, ((BracketBlockManager)manager).bracketAnchor.parent, root);

        if (block.child != null)
            Deserialize(list, block.child, manager.outAnchor.parent, root);
    }
}
