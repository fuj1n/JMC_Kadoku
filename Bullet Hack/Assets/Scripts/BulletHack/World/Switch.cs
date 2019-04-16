using System.Collections;
using BulletHack.FX;
using BulletHack.World.Event;
using UnityEngine;

namespace BulletHack.World
{
    public class Switch : EventCallback
    {
        [ColorUsage(false)]
        public Color offColor = Color.red;
        [ColorUsage(false)]
        public Color onColor = Color.green;

        public float fadeTime = 1F;
        
        [HideInInspector]
        public bool state;

        private SineFader[] faders;
        
        private void Awake()
        {
            faders = GetComponentsInChildren<SineFader>(true);

            StartCoroutine(Fade(offColor, offColor, fadeTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || state)
                return;

            state = true;
            Execute();
            StartCoroutine(Fade(offColor, onColor));
        }

        private IEnumerator Fade(Color start, Color end, float time = 0F)
        {
            start.a = 1F;
            end.a = 1F;
            
            do
            {
                time += Time.deltaTime;
                foreach (SineFader fader in faders)
                {
                    if (!fader)
                        continue;

                    fader.colorA = Color.Lerp(start, end, time / fadeTime);
                    fader.colorB = fader.colorA * 0.5F;
                }

                yield return null;
            } while (time < fadeTime);
        }
    }
}
