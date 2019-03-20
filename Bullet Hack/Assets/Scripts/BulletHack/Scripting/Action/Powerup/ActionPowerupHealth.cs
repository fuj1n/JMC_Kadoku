using BulletHack.UI;
using UnityEngine;

namespace BulletHack.Scripting.Action.Powerup
{
    [BlockLoader.Block("powerup.heal")]
    public class ActionPowerupHealth : ActionPowerup
    {
        public override string GetName() => "Restore Health";

        public override void Powerup()
        {
            CombatManager.Instance.Script.currentAvatar.Health++;
        }

        public override int GetPowerupCount() => CombatManager.Instance.Script.currentAvatar.powerups.health;

        public override void DecrPowerupCount() => CombatManager.Instance.Script.currentAvatar.powerups.health--;

        public override Color GetColor() => Color.green;
    }
}