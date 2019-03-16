using UnityEngine;

namespace BulletHack.World
{
    public class CameraSwitcher : MonoBehaviour
    {
        private Camera cam;

        private void Awake()
        {
            cam = GetComponentInChildren<Camera>();

            if (!cam)
            {
                Debug.LogError("No camera found on " + name);
                Destroy(gameObject);
                return;
            }

            cam.enabled = false;
        }

        private void OnTriggerStay(Collider other)
        {
            CameraSystem.Instance.SetActiveCamera(cam);
        }
    }
}
