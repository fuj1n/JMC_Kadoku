using JetBrains.Annotations;
using UnityEngine;

namespace BulletHack.World
{
    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem Instance { get; private set; }
        
        private Camera NullCam
        {
            get
            {
                if (!nullCam)
                    nullCam = new Camera {enabled = false, name = "NULL Camera"};
            
                Debug.LogWarning("[WARN] No active camera, returning null camera");
                return nullCam;
            }
        }
        private Camera nullCam;

        private void Awake()
        {
            Instance = this;
        }

        [NotNull]
        public Camera GetActiveCamera()
        {
            return Camera.main != null ? Camera.main : NullCam;
        }
    }
}
