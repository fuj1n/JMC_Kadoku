using UnityEngine;

namespace BulletHack.FX
{
    public class ScanLine : MonoBehaviour
    {
        public Vector2 direction = new Vector2(1, 0);

        private Renderer render;
        
        private void Awake()
        {
            render = GetComponent<Renderer>();
        }

        private void Update()
        {
            if (!render)
                return;

            Vector2 offset = render.material.mainTextureOffset;

            offset += direction * Time.deltaTime;
            if (offset.x > 1F)
                offset.x -= 1F;
            else if (offset.x < -1F)
                offset.x += 1F;
            if (offset.y > 1F)
                offset.y -= 1F;
            else if (offset.y < -1F)
                offset.y += 1F;
            
            render.material.mainTextureOffset = offset;
        }
    }
}
