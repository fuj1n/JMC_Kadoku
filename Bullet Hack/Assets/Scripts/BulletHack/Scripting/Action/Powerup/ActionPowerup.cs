namespace BulletHack.Scripting.Action.Powerup
{
    public abstract class ActionPowerup : ActionBase
    {
        public override void Execute()
        {
            if (!GameData.Instance)
                return;
            
            if (GameData.Instance.powerups[GetPowerupType()] > 0)
            {
                GameData.Instance.powerups[GetPowerupType()]--;
                Powerup();
            }
            else
            {
                CombatManager.Instance.Script.currentAvatar.Shake();
            }
        }

        public abstract void Powerup();
        public abstract Entity.Ticking.Powerup.PowerupType GetPowerupType();
    }
}