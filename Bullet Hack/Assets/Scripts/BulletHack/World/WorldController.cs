using UnityEngine;

namespace BulletHack.World
{
    public class WorldController : MonoBehaviour
    {
        private static WorldController instance;
        
        public GameObject theWorld;

        private void Awake()
        {
            instance = this;
        }

        public static bool Enable()
        {
            if (!instance)
                return false;
            
            instance.theWorld.SetActive(true);

            return true;
        }

        public static bool Disable()
        {
            if (!instance)
                return false;
            
            instance.theWorld.SetActive(false);

            return true;
        }
    }
}
