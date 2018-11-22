using UnityEngine;

namespace Kadoku.Scripting.Actions
{
    public class StartAction : IDrawnAction
    {
        public static readonly Socket[] sockets =
        {
            new Socket(0F, 1F, false)
        };

        public string GetName() => "Start";
        public Vector2Int GetSize() => new Vector2Int(32, 16);
        public Color GetColor() => Color.green;

        public Socket[] GetSockets() => sockets;
        public RenderType GetRenderType() => RenderType.NORMAL;
    }
}