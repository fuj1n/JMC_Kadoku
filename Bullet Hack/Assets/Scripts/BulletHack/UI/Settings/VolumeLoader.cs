using UnityEngine;
using UnityEngine.UI;

namespace BulletHack.UI.Settings
{
    public class VolumeLoader : MonoBehaviour
    {
        public SoundManager.Channel channel;
        
        private void OnEnable()
        {
            Slider slider = GetComponent<Slider>();

            if (slider)
                slider.value = SoundManager.GetVolume(channel);
        }
    }
}
