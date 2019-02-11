using BulletHack.UI;

namespace BulletHack.Scripting.Action
{
    [BlockLoader.Block]
    public class ActionShoot : ActionBase
    {
        public override void Execute()
        {
            CombatManager.Instance.Script.currentAvatar.Shoot();
        }

        public override string GetName() => "Fire";
    }
}