using UnityEngine;

namespace BulletHack.FX
{
    public class SineFader : MonoBehaviour
    {
        public float amplitude = 1F;
        public float frequency = 1F;

        [ColorUsage(false)]
        public Color colorA = Color.black;
        [ColorUsage(false)]
        public Color colorB = Color.red;

        public bool randomizeStartingAngle = true;
        
        private Renderer render;

        private float angle = 0F;
        private static readonly int emissionColor = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            render = GetComponent<Renderer>();

            if (randomizeStartingAngle)
                angle = Random.Range(0F, 2 * Mathf.PI);
        }

        private void Update()
        {
            if (!render)
                return;

            angle += Time.deltaTime;
            float alpha = Mathf.Clamp01(Mathf.Sin(angle * frequency) * amplitude);
            
            render.material.SetColor(emissionColor, Color.Lerp(colorA, colorB, alpha));

            if (angle > 2 * Mathf.PI)
                angle -= 2 * Mathf.PI;
        }
    }
}
