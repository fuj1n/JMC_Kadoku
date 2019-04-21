using BulletHack.UI;
using UnityEngine;

namespace BulletHack.Scripting.Action.Powerup
{
    [BlockLoader.Block("powerup.shield")]
    public class ActionPowerupShield : ActionPowerup
    {
        public override string GetName() => "Shield";

        public override void Powerup()
        {
            CombatManager.Instance.Script.currentAvatar.powerups.ShieldActive = true;
        }

        public override Color GetColor() => Color.yellow;
        public override Entity.Ticking.Powerup.PowerupType GetPowerupType() => Entity.Ticking.Powerup.PowerupType.Shield;
    }
}