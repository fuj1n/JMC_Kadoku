using BulletHack.UI;
using UnityEngine;

namespace BulletHack.World.Messaging
{
    public class CreatePopup : MonoBehaviour
    {
        public PopupMessageUi.PopupData data;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PopupMessageUi.Open(data, OnPopupClose);
            }
        }

        private void OnPopupClose()
        {
            foreach(IPopupCloseHandler handler in GetComponentsInParent<IPopupCloseHandler>(true))   
                    handler.OnPopupClosed();
            Destroy(this);
        }

        public interface IPopupCloseHandler
        {
            void OnPopupClosed();
        }
    }
}
