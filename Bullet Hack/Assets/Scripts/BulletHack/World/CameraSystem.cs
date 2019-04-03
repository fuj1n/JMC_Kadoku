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
                if (nullCam == null)
                {
                    nullCam = new GameObject("NULL Camera").AddComponent<Camera>();
                    nullCam.gameObject.SetActive(false);
                }

                Debug.LogWarning("[WARN] No active camera, returning null camera");
                return nullCam;
            }
        }

        private Camera nullCam;
        private Camera activeCamera;
        
        private void Awake()
        {
            Instance = this;
        }

        [NotNull]
        public Camera GetActiveCamera()
        {
            return activeCamera != null ? activeCamera : NullCam;
        }

        public void SetActiveCamera(Camera c)
        {
            if (c == activeCamera)
                return;
            
            if (activeCamera)
                activeCamera.enabled = false;
            activeCamera = c;
            activeCamera.enabled = true;
        }
    }
}