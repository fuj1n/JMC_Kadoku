using UnityEditor;
using UnityEngine;

namespace BulletHack.Editor
{
    public static class EditorUtils
    {
        [MenuItem("Utilities/Print World Pos")]
        private static void PrintWorldPos()
        {
            if(Selection.activeTransform)
                Debug.Log(Selection.activeTransform.position);
        }
    }
}
