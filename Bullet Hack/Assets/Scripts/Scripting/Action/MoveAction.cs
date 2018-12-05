using UnityEngine;

[BlockLoader.Block(BlockLoader.BlockAttribute.BlockType.STANDARD)]
public class MoveAction : ActionBase
{
    [InputVar]
    private Direction direction = Direction.RIGHT;
    [InputVar]
    private bool test = true;
    [InputVar]
    private int testInt = 42;
    [InputVar]
    private string testStr = "Hello, World!";

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
