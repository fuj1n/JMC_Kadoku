using UnityEngine;
using UnityEngine.UI;

namespace BulletHack.FX
{
    public class GaussianFade : MonoBehaviour
    {
        public float frequency = 1F;
        public float amplitude = 1F;

        public bool useScaledTime = true;

        private Graphic graphic;

        private void Awake()
        {
            graphic = GetComponent<Graphic>();
        }

        private void Update()
        {
            Color c = graphic.color;
            c.a = Mathf.Clamp01(amplitude * Mathf.PerlinNoise(useScaledTime ? Time.time : Time.realtimeSinceStartup * frequency, 0F));
            graphic.color = c;
        }
    }
}
