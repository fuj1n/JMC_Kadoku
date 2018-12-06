[BlockLoader.Block(BlockLoader.BlockAttribute.BlockType.BRACKET)]
public class RepeatTimesBlock : ActionBlockBase {
    [InputVar]
    private int times;

    private int currentTimes;

    public override string GetName() => "Repeat";

    public override bool ShouldExecute()
    {
        return currentTimes < times;
    }

    public override void Next()
    {
        currentTimes++;
    }

    public override void ResetState()
    {
        base.ResetState();

        currentTimes = 0;
    }
}
