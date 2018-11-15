using Kadoku.Scripting.Arguments;
using UnityEngine;

namespace Kadoku.Scripting.Actions
{
    public abstract class BaseAction : IDrawnAction
    {
        public static readonly Socket[] sockets =
        {
            new Socket(.1F, 0F, true),
            new Socket(0F, 1F, false)
        };

        public abstract string GetName();
        public abstract Vector2Int GetSize();
        public virtual Color GetColor() => Color.white;

        public Socket[] GetSockets() => sockets;
        public virtual RenderType GetRenderType() => RenderType.NORMAL;

        public virtual BaseArgument[] GetArguments() => null;
    }
}