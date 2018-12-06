using UnityEngine;

[BlockLoader.Block(BlockLoader.BlockAttribute.BlockType.STANDARD)]
public class MoveAction : ActionBase
{
    [InputVar]
    private Direction direction = Direction.RIGHT;

    public override void Execute()
    {
        ScriptableCharacter character = CombatManager.Instance.Script.currentAvatar;

        switch (direction)
        {
            case Direction.UP:
                character.Y--;
                break;
            case Direction.DOWN:
                character.Y++;
                break;
            case Direction.LEFT:
                character.X--;
                break;
            case Direction.RIGHT:
                character.X++;
                break;
        }

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
