using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace BulletHack.FX
{
    public class Alarm : MonoBehaviour
    {
        public PostProcessVolume volume;

        private ColorGrading grading;

        private void Awake()
        {
            volume.profile.TryGetSettings<ColorGrading>(out grading);
            
            if(!grading)
                Destroy(this);
        }

        private void Update()
        {
            // TODO
        }
    }
}
