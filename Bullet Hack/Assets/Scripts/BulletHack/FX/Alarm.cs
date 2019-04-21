using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace BulletHack.FX
{
    public class Alarm : MonoBehaviour
    {
        public PostProcessVolume volume;

        public Vector4 min;
        public Vector4 max;

        [Header("Wave")]
        public float amplitude = 1F;
        public float frequency = 1F;
        public float offset = 0F;
        
        private ColorGrading grading;
        private float angle = 0F;
        
        private void Awake()
        {
            volume.profile.TryGetSettings(out grading);
            
            if(!grading)
                Destroy(this);
        }

        private void Update()
        {
            angle += Time.deltaTime;
            float alpha = Mathf.Clamp01(offset + Mathf.Sin(angle * frequency) * amplitude);

            grading.lift.Interp(min, max, alpha);

            if (angle > 2 * Mathf.PI)
                angle -= 2 * Mathf.PI;
        }
    }
}
