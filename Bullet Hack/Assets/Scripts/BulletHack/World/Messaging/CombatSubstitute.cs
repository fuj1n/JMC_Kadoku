using BulletHack.World.Event;

namespace BulletHack.World.Messaging
{
    public class CombatSubstitute : EventCallback, ICombatEntryEvent
    {
        public bool OnPreCombatEnter()
        {
            Execute();
            Destroy(gameObject);
            return true;
        }
    }
}