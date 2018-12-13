using UnityEngine;

[BlockLoader.Block]
public class MoveAction : ActionBase
{
    [InputVar]
    private Direction direction;

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
