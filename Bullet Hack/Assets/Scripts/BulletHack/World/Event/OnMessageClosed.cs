using BulletHack.World.Messaging;

namespace BulletHack.World.Event
{
    public class OnMessageClosed : EventCallback, CreatePopup.IPopupCloseHandler
    {
        public void OnPopupClosed() => Execute();
    }
}
