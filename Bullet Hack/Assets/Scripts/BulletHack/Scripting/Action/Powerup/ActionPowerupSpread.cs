using BulletHack.UI;
using UnityEngine;

namespace BulletHack.Scripting.Action.Powerup
{
    [BlockLoader.Block("powerup.spread")]
    public class ActionPowerupSpread : ActionPowerup
    {
        public override string GetName() => "Spread Shot";

        public override void Powerup()
        {
            for (int i = -2; i <= 2; i++)
                CombatManager.Instance.Script.currentAvatar.Shoot(i);
        }

        public override Color GetColor() => Color.red;
        public override Entity.Ticking.Powerup.PowerupType GetPowerupType() => Entity.Ticking.Powerup.PowerupType.Spread;
    }
}