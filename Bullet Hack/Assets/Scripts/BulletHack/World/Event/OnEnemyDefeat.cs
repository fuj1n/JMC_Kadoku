using JetBrains.Annotations;

namespace BulletHack.World.Event
{
    public class OnEnemyDefeat : EventCallback
    {
        [UsedImplicitly]
        private void OnEnemyDefeated() => Execute();
    }
}
