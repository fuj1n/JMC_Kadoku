public abstract class ActionBlockBase : ActionBase
{
    private ActionBase currentInstruction;

    public override void Execute()
    {
        if (!currentInstruction && ShouldExecute())
        {
            Next();
            currentInstruction = ((BracketBlockManager)manager).bracketConnector?.GetComponent<ActionBase>();
        }

        if (!currentInstruction)
            return;

        currentInstruction.Execute();
        currentInstruction = currentInstruction.GetNextAction();
    }

    public override ActionBase GetNextAction()
    {
        if (currentInstruction || ShouldExecute())
            return this;
        return base.GetNextAction();
    }

    public override BlockManagerBase GetNextActionRaw()
    {
        if (currentInstruction || ShouldExecute())
            return manager;

        ResetState();
        return base.GetNextActionRaw();
    }

    public abstract bool ShouldExecute();
    public abstract void Next();

    public override void ResetState()
    {
        base.ResetState();

        if (manager is BracketBlockManager)
            ((BracketBlockManager)manager).GetBracketConnection()?.GetComponent<ActionBlockBase>()?.ResetState();

        currentInstruction = null;
    }
}
