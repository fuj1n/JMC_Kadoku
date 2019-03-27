using UnityEngine;

namespace BulletHack.FX
{
    public class TextureAnimation : MonoBehaviour
    {
        public int fps = 15;
        public Vector2Int frameCount;
        public Vector2 textureSize;

        private Material mat;
        
        private float Spf => 1F / fps;
        
        private float nextFrame = 0F;
        private int frame = 0;

        private void Awake()
        {
            mat = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            nextFrame -= Time.deltaTime;
            if (nextFrame <= 0)
            {
                nextFrame = Spf;
                
                mat.mainTextureOffset = new Vector2(Mathf.FloorToInt(frame % frameCount.x), Mathf.FloorToInt((float)frame / frameCount.x)) * textureSize;
                
                frame++;
                if (frame > frameCount.x * frameCount.y)
                    frame = 0;
            }
        }
    }
}
