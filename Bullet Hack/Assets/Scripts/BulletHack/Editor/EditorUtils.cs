using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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

        [MenuItem("Utilities/Print Lift Value")]
        private static void PrintGradingValue()
        {
            if (Selection.activeGameObject)
            {
                PostProcessVolume volume = Selection.activeGameObject.GetComponent<PostProcessVolume>();
                if (volume && volume.sharedProfile)
                    if(volume.sharedProfile.TryGetSettings(out ColorGrading grading))
                        Debug.Log(grading.lift.value);
            }
        }
    }
}
