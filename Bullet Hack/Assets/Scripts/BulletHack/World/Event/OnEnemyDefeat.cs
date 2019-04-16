using BulletHack.World.Enemy.BattleEntry;

namespace BulletHack.World.Event
{
    public class OnEnemyDefeat : EventCallback, BattleEntryBase.IEnemyDefeatedHandler
    {
        public void OnEnemyDefeated() => Execute();
    }
}
