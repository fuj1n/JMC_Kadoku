[BlockLoader.Block]
public class AwaitAction : ActionBase
{
    [InputVar(Reverse = true)]
    private int turns = 1;

    private int takenTurns = 0;

    public override void Execute()
    {
        UnityEngine.Debug.Log("Wait");
        takenTurns++;
    }

    public override ActionBase GetNextAction()
    {
        if (takenTurns < turns)
            return this;

        return base.GetNextAction();
    }

    public override BlockManagerBase GetNextActionRaw()
    {
        if (takenTurns < turns)
            return manager;

        return base.GetNextActionRaw();
    }

    public override string GetName() => "Wait";
}
