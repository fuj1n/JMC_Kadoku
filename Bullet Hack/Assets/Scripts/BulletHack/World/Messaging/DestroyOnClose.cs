using UnityEngine;

namespace BulletHack.World.Messaging
{
    public class DestroyOnClose : MonoBehaviour
    {
        public Object[] targets = {};

        private void OnPopupClosed()
        {
            foreach (Object obj in targets)
                Destroy(obj);
        }
    }
}
