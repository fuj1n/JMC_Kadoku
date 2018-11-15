namespace Kadoku.Scripting.Arguments
{
    public abstract class BaseArgument
    {
        public readonly string name;

        public BaseArgument(string name)
        {
            this.name = name;
        }
    }
}