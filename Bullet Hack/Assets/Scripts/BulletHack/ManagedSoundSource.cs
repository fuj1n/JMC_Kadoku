using UnityEngine;

namespace BulletHack
{
    public class ManagedSoundSource : MonoBehaviour
    {
        public AudioClip clip;
        public SoundManager.Channel channel;
        [Range(0F, 2F)]
        public float volumeMultiplier = 1F;

        public bool loop;
        public bool moveToManager = true;
        
        private AudioSource source;

        private void Awake()
        {
            source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = loop;
            source.Play();
        }

        private void Update()
        {
            if (moveToManager)
                transform.position = SoundManager.Position;
            
            if (!source)
                return;

            source.volume = Mathf.Clamp01(SoundManager.GetVolume(channel) * volumeMultiplier);
        }

        private void OnEnable()
        {
            if (source)
                source.enabled = true;
        }

        private void OnDisable()
        {
            if(source)
                source.enabled = false;
        }
    }
}
