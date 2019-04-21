using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace BulletHack.World
{
    public class Door : MonoBehaviour
    {
        public float fadeTime = 1F;

        public AudioClip unpowerSound;
        
        private bool openScheduled;
        
        [UsedImplicitly]
        public void Open()
        {
            openScheduled = true;
        }

        private void Update()
        {
            if (openScheduled)
            {
                if(unpowerSound)
                    SoundManager.PlayClip(unpowerSound, SoundManager.Channel.SoundEffect);
                
                StartCoroutine(FadeOut());
                openScheduled = false;
            }
        }

        private IEnumerator FadeOut()
        {
            foreach (Collider c in GetComponentsInChildren<Collider>())
                c.enabled = false;

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            
            float time = 0F;

            while (time < fadeTime)
            {
                time += Time.deltaTime;

                foreach(Renderer r in renderers)
                {
                    Color c = r.material.color;
                    c.a = Mathf.Lerp(1F, 0F, time / fadeTime);
                    r.material.color = c;
                }
                
                yield return null;
            }
            
            Destroy(gameObject);
        }
    }
}
