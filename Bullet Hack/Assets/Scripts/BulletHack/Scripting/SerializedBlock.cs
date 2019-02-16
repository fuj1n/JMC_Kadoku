using System.Collections.Generic;
using System.Linq;
using BulletHack.Scripting.Action;
using BulletHack.UI;
using BulletHack.UI.Binder;
using BulletHack.UI.BlockManager;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace BulletHack.Scripting
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class SerializedBlock
    {
        public string id;
        public Dictionary<string, object> values = new Dictionary<string, object>();
        public SerializedBlock child;

        public SerializedBlock blockIn;

        public SerializedBlock()
        {
        }

        public SerializedBlock(string id) : this()
        {
            this.id = id;
        }

        public static string Serialize(BlockManagerBase manager)
        {
            return !manager ? "" : JsonConvert.SerializeObject(Serialize(manager, true), Formatting.Indented);
        }

        public static SerializedBlock Serialize(BlockManagerBase manager, bool partial)
        {
            if (!manager)
                return null;

            SerializedBlock root = SerializeSingle(manager);

            if (manager is BracketBlockManager && ((BracketBlockManager) manager).GetBracketConnection())
                Serialize(root, ((BracketBlockManager) manager).GetBracketConnection(), true);

            if (manager.GetOutConnection())
                Serialize(root, manager.GetOutConnection());

            return root;
        }

        private static void Serialize(SerializedBlock block, BlockManagerBase manager, bool bracket = false)
        {
            while (true)
            {
                if (!manager) return;

                SerializedBlock next;

                if (bracket)
                {
                    block.blockIn = SerializeSingle(manager);

                    next = block.blockIn;
                }
                else
                {
                    block.child = SerializeSingle(manager);

                    next = block.child;
                }

                if (manager is BracketBlockManager && ((BracketBlockManager) manager).GetBracketConnection())
                    Serialize(next, ((BracketBlockManager) manager).GetBracketConnection(), true);

                if (manager.GetOutConnection())
                {
                    block = next;
                    manager = manager.GetOutConnection();
                    bracket = false;
                    continue;
                }

                break;
            }
        }

        public static SerializedBlock SerializeSingle(BlockManagerBase manager)
        {
            if (!manager)
                return null;

            ActionBase action = manager.GetComponent<ActionBase>();

            SerializedBlock block = new SerializedBlock(action.Id);

            foreach (ValueBinder binder in action.transform.Find("Vars").GetComponentsInChildren<ValueBinder>())
            {
                object value = binder.field.GetValue(binder.obj);

                if (binder.field.FieldType.IsEnum)
                {
                    System.Array vals = System.Enum.GetValues(binder.field.FieldType);
                    List<string> names = System.Enum.GetNames(binder.field.FieldType).ToList();

                    block.values.Add(binder.field.Name, vals.GetValue(names.IndexOf(System.Enum.GetName(binder.field.FieldType, value))));
                }
                else
                {
                    block.values.Add(binder.field.Name, value);
                }
            }

            return block;
        }

        public static BlockManagerBase Deserialize(string s, Transform root)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;

            SerializedBlock block = JsonConvert.DeserializeObject<SerializedBlock>(s);

            return Deserialize(block, root);
        }

        public static BlockManagerBase Deserialize(SerializedBlock block, Transform root)
        {
            if (block == null)
                return null;

            CodeBlockDrag drag = BlockLoader.CreateBlock(block.id, root).GetComponent<CodeBlockDrag>();
            BlockManagerBase manager = drag.GetComponent<BlockManagerBase>();

            foreach (ValueBinder binder in drag.GetComponentsInChildren<ValueBinder>())
            {
                if (block.values.ContainsKey(binder.field.Name))
                {
                    object value = block.values[binder.field.Name];

                    if (value is long && (long) value >= int.MinValue && (long) value <= int.MaxValue)
                        value = System.Convert.ToInt32(value);

                    if (binder.field.FieldType.IsEnum)
                        binder.field.SetValue(binder.obj, System.Enum.ToObject(binder.field.FieldType, value));
                    else
                        binder.field.SetValue(binder.obj, System.Convert.ChangeType(value, binder.field.FieldType));
                }
            }

            if (manager is BracketBlockManager && block.blockIn != null)
                Deserialize(block.blockIn, ((BracketBlockManager) manager).bracketAnchor.parent, root);

            if (block.child != null)
                Deserialize(block.child, manager.outAnchor.parent, root);

            return manager;
        }

        private static void Deserialize(SerializedBlock block, Transform outConnector, Transform root)
        {
            while (true)
            {
                CodeBlockDrag drag = BlockLoader.CreateBlock(block.id, root).GetComponent<CodeBlockDrag>();
                drag.ConnectTo(outConnector);

                BlockManagerBase manager = drag.GetComponent<BlockManagerBase>();

                foreach (ValueBinder binder in drag.GetComponentsInChildren<ValueBinder>())
                {
                    if (block.values.ContainsKey(binder.field.Name))
                    {
                        object value = block.values[binder.field.Name];

                        if (value is long && (long) value >= int.MinValue && (long) value <= int.MaxValue) value = System.Convert.ToInt32(value);

                        if (binder.field.FieldType.IsEnum)
                            binder.field.SetValue(binder.obj, System.Enum.ToObject(binder.field.FieldType, value));
                        else
                            binder.field.SetValue(binder.obj, System.Convert.ChangeType(value, binder.field.FieldType));
                    }
                }

                if (manager is BracketBlockManager && block.blockIn != null)
                    Deserialize(block.blockIn, ((BracketBlockManager) manager).bracketAnchor.parent, root);

                if (block.child != null)
                {
                    block = block.child;
                    outConnector = manager.outAnchor.parent;
                    continue;
                }

                break;
            }
        }

        public static BlockManagerBase Clone(BlockManagerBase manager, Transform newRoot = null)
        {
            if (newRoot == null)
                newRoot = manager.GetComponent<CodeBlockDrag>().root;

            return Deserialize(Serialize(manager), newRoot);
        }
    }
}