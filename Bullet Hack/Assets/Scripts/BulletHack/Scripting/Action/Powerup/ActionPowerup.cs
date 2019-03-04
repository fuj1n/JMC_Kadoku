namespace BulletHack.Scripting.Action.Powerup
{
    public abstract class ActionPowerup : ActionBase
    {
        public override void Execute()
        {
            if (GetPowerupCount() > 0)
            {
                DecrPowerupCount();
                Powerup();
            }
            else
            {
                CombatManager.Instance.Script.currentAvatar.Shake();
            }
        }

        public abstract void Powerup();
        public abstract int GetPowerupCount();
        public abstract void DecrPowerupCount();
    }
}