using UnityEngine;

namespace Kadoku.Scripting.Actions
{
    public interface IDrawnAction
    {
        string GetName();
        Vector2Int GetSize();
        Socket[] GetSockets();
        Color GetColor();
        RenderType GetRenderType();
    }

    public enum RenderType
    {
        NORMAL,
        BLOCK
    }
}