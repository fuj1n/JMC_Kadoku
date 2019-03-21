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

        public override int GetPowerupCount() => CombatManager.Instance.Script.currentAvatar.powerups.shield;

        public override void DecrPowerupCount() => CombatManager.Instance.Script.currentAvatar.powerups.shield--;

        public override Color GetColor() => Color.yellow;
    }
}