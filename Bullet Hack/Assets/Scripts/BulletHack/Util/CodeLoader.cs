using BulletHack.Scripting;
using BulletHack.UI;
using BulletHack.UI.Blocks;
using UnityEngine;

namespace BulletHack.Util
{
    public class CodeLoader : MonoBehaviour
    {
        [Multiline]
        public string code;
        
        private BlockManagerBase manager;
        private Transform root;
        
        private void Awake()
        {
            manager = GetComponent<BlockManagerBase>();
            root = manager.GetComponent<CodeBlockDrag>().root;
            
            LoadCode();
        }
        
        public void LoadCode()
        {
            if (manager.outConnector)
            {
                BlockManager man = manager.outConnector;
                manager.Disconnect(man);
                Destroy(man.gameObject);
            }

            SerializedBlock.Deserialize(code, root).GetComponent<CodeBlockDrag>().ConnectTo(manager.outAnchor.parent);
        }
    }
}
