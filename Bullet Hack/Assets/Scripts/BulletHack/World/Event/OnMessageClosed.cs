using JetBrains.Annotations;
using UIEventDelegate;
using UnityEngine;

namespace BulletHack.World.Event
{
    public class OnMessageClosed : EventCallback
    {
        [UsedImplicitly]
        private void OnPopupClosed() => Execute();
    }
}
