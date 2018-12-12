[BlockLoader.Block]
public class RepeatForeverBlock : ActionBlockBase
{
    public override string GetName() => "Repeat Forever";

    public override void Next() { }

    public override bool ShouldExecute()
    {
        return true;
    }
}
