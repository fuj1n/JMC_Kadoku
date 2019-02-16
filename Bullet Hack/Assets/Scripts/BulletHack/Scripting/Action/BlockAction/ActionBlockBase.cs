using BulletHack.UI.Blocks;

namespace BulletHack.Scripting.Action.BlockAction
{
    public abstract class ActionBlockBase : ActionBase
    {
        private ActionBase currentInstruction;

        private bool isBroken;

        public override void Execute()
        {
            if (!currentInstruction && ShouldExecute())
            {
                Next();
                currentInstruction = ((BracketBlockManager) manager).bracketConnector?.GetComponent<ActionBase>();
            }

            if (!currentInstruction)
                return;

            currentInstruction.Execute();

            ScriptController script = CombatManager.Instance.Script;

            currentInstruction.GetManager().FadeOutline(0F, script.GetTweenSpeed() * .5F);
            currentInstruction = currentInstruction.GetNextAction();
            if (currentInstruction && !isBroken)
                currentInstruction.GetManager().SetOutline(script.runningHighlight, script.GetTweenSpeed() * .5F);
        }

        public override ActionBase GetNextAction()
        {
            if (!isBroken && (currentInstruction || ShouldExecute()))
                return this;
            return base.GetNextAction();
        }

        public override BlockManagerBase GetNextActionRaw()
        {
            if (!isBroken && (currentInstruction || ShouldExecute()))
                return manager;

            ResetState();
            return base.GetNextActionRaw();
        }

        public abstract bool ShouldExecute();
        public abstract void Next();

        public bool Break()
        {
            isBroken = CanBreak();

            return CanBreak();
        }

        protected virtual bool CanBreak() => true;

        public override void ResetState()
        {
            base.ResetState();

            isBroken = false;

            if (manager is BracketBlockManager)
                ((BracketBlockManager) manager).GetBracketConnection()?.GetComponent<ActionBlockBase>()?.ResetState();

            currentInstruction = null;
        }
    }
}