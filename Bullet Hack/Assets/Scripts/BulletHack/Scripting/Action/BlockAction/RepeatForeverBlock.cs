using BulletHack.UI;

namespace BulletHack.Scripting.Action.BlockAction
{
    [BlockLoader.Block("repeat.forever")]
    public class RepeatForeverBlock : ActionBlockBase
    {
        public override string GetName() => "Repeat Forever";

        public override void Next()
        {
        }

        public override bool ShouldExecute()
        {
            return true;
        }
    }
}