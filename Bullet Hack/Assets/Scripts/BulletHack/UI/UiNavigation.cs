﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletHack.UI
{
    public class UiNavigation : MonoBehaviour
    {
        public GameObject settings;
        public AudioClip menuClick;

        private void Awake()
        {
            if(settings)
                settings.SetActive(false);
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void OpenSettings()
        {
            if(settings)
                settings.SetActive(true);
        }

        public void CloseSettings()
        {
            if(settings)
                settings.SetActive(false);

            SettingsData.Flush();
        }
        
        public void ExitGame()
        {
            Application.Quit();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        public void ClickSound()
        {
            ClickSoundOn((int)SoundManager.Channel.SoundEffect);
        }

        public void ClickSoundOn(int channel)
        {
            if(menuClick)
                SoundManager.PlayClip(menuClick, (SoundManager.Channel)channel);
        }

        public void ClearData()
        {
            GameData.DestroyActiveInstance();
        }
        
        #region Settings

        public void SetVolume(SoundManager.Channel channel, float value) => SoundManager.SetVolume(channel, value);

        public void SetMaster(float value) => SetVolume(SoundManager.Channel.Master, value);
        public void SetMusic(float value) => SetVolume(SoundManager.Channel.Music, value);
        public void SetSfx(float value) => SetVolume(SoundManager.Channel.SoundEffect, value);

        #endregion
    }
}
