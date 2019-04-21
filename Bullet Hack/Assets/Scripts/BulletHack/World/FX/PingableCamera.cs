using UnityEngine;

namespace BulletHack.World.FX
{
    public class PingableCamera : MonoBehaviour
    {
        public float displayTime = 2F;

        private float lastPing = -9999F;
        private new Camera camera;
        
        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (!camera)
                return;

            bool display = Time.time - lastPing <= displayTime;
            if (camera.enabled != display)
                camera.enabled = display;
        }

        public void Ping()
            => lastPing = Time.time;
    }
}
