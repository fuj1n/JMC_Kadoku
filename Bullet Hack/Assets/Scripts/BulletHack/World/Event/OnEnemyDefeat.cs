using JetBrains.Annotations;
using UIEventDelegate;
using UnityEngine;

namespace BulletHack.World.Event
{
    public class OnEnemyDefeat : EventCallback
    {
        [UsedImplicitly]
        private void OnEnemyDefeated() => Execute();
    }
}
