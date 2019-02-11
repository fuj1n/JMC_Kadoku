using BulletHack.UI;
using BulletHack.UI.BlockManager;

namespace BulletHack.Scripting.Action
{
    [BlockLoader.Block]
    public class AwaitAction : ActionBase
    {
        [InputVar(Reverse = true)]
        private int turns = 1;

        private int takenTurns;

        public override void Execute()
        {
            UnityEngine.Debug.Log("Wait");
            takenTurns++;
        }

        public override ActionBase GetNextAction()
        {
            return takenTurns < turns ? this : base.GetNextAction();
        }

        public override BlockManagerBase GetNextActionRaw()
        {
            return takenTurns < turns ? manager : base.GetNextActionRaw();
        }

        public override string GetName() => "Wait";
    }
}