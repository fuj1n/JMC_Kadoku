using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHack.UI
{
    public class CodeRunningOverlay : MonoBehaviour
    {
        public float fadeTime = 0.5F;

        private Image image;

        private float onAlpha;
        private bool cachedRunning;

        private void Awake()
        {
            image = GetComponent<Image>();

            onAlpha = image.color.a;

            image.DOFade(0F, 0F);
            image.enabled = true;
        }

        private void Update()
        {
            bool isRunning = CombatManager.Instance.Script.IsRunning;

            if (isRunning != cachedRunning)
            {
                cachedRunning = isRunning;
                image.DOFade(isRunning ? onAlpha : 0F, fadeTime);
            }
        }
    }
}