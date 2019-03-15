using System;
using BulletHack.UI;

namespace BulletHack.Scripting.Action
{
    [BlockLoader.Block("move")]
    public class MoveAction : ActionBase
    {
        [InputVar]
        private Direction direction;

        public override void Execute()
        {
            ScriptableCharacter character = CombatManager.Instance.Script.currentAvatar;

            switch (direction)
            {
                case Direction.Up:
                    character.Y--;
                    break;
                case Direction.Down:
                    character.Y++;
                    break;
                case Direction.Left:
                    character.X--;
                    break;
                case Direction.Right:
                    character.X++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string GetName() => "Move";

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}