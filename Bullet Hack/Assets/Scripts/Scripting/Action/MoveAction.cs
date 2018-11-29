using UnityEngine;

[BlockLoader.Block(BlockLoader.BlockAttribute.BlockType.STANDARD)]
public class MoveAction : ActionBase {
    [InputVar]
    private Direction direction;

    public override void Execute()
    {
        Debug.Log(direction);
    }

    public override string GetName() => "Move";

    private enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}
