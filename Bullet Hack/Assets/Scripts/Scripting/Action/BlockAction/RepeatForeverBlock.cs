[BlockLoader.Block(BlockLoader.BlockAttribute.BlockType.BRACKET)]
public class RepeatForeverBlock : ActionBlockBase
{
    public override string GetName() => "Repeat Forever";

    public override void Next() { }

    public override bool ShouldExecute()
    {
        return true;
    }
}
