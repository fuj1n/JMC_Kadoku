namespace BulletHack.World.Messaging
{
    public interface ICombatEntryEvent
    {
        /// <summary>
        /// Called before the combat is entered
        /// </summary>
        /// <returns>Whether combat entry should be cancelled</returns>
        bool OnPreCombatEnter();
    }
}