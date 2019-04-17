using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BulletHack
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager instance;
        
        private static SoundSettings soundSettings = new SoundSettings();
        
        public static void PlayClip(AudioClip clip, Channel channel)
                // ReSharper disable once Unity.NoNullPropogation
            => PlayClipAtPoint(clip, channel, instance?.transform.position ?? Vector3.zero);

        public static void PlayClipAtPoint(AudioClip clip, Channel channel, Vector3 point)
        {
            float volume = GetVolume(Channel.Master);

            if (channel == Channel.Master)
                Debug.LogWarningFormat("Playing sound {0} on the master channel, please use one of the other channels", clip.name);
            else
                volume *= GetVolume(channel);

            AudioSource.PlayClipAtPoint(clip, point, volume);
        }
        
        public static void SetVolume(Channel channel, float volume)
        {
            soundSettings.volume[channel] = Mathf.Clamp01(volume);

            SettingsData.SetValue("sound", soundSettings);
        }

        public static float GetVolume(Channel channel)
            => !soundSettings.volume.ContainsKey(channel) ? 0F : soundSettings.volume[channel];

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            SettingsData.RegisterListener("sound", Load);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Create()
        {
            GameObject go = new GameObject("Sound Manager");
            instance = go.AddComponent<SoundManager>();
            go.AddComponent<AudioListener>();
            DontDestroyOnLoad(go);   
        }

        private static void Load(object value)
        {
            if(value is SoundSettings)
                soundSettings = (SoundSettings) value;
        }

        [Serializable]
        public enum Channel
        {
            Music,
            SoundEffect,
            Master = -9999
        }

        public class SoundSettings
        {
            public Dictionary<Channel, float> volume;

            public SoundSettings()
            {
                volume = new Dictionary<Channel, float>();
                foreach (Channel c in Enum.GetValues(typeof(Channel)))
                    volume[c] = 1F;
            }
        }
    }
}