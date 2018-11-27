public abstract class ActionBlockBase : ActionBase
{
    private ActionBase currentInstruction;

    public override void Execute()
    {
        if (!currentInstruction && ShouldExecute())
        {
            currentInstruction = ((BracketBlockManager)manager).bracketConnector.GetComponent<ActionBase>();
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
        return base.GetNextActionRaw();
    }

    public abstract bool ShouldExecute();

    public override void ResetState()
    {
        base.ResetState();
        currentInstruction = null;
    }
}
