using UnityEngine;

namespace BulletHack.FX
{
    public class TextureAnimation : MonoBehaviour
    {
        public int fps = 15;
        public Vector2Int frameCount;
        public Vector2 textureSize;
        
        private float Spf => 1F / fps;
        
        private void Update()
        {
            
        }
    }
}
