using UnityEngine;

namespace BulletHack.World
{
    public class CameraRail : MonoBehaviour
    {
        public Vector3 min;
        public Vector3 max;
        public Vector3 playerOffset;

        private Transform player;
        private Vector3 intendedPosition;

        private Vector3 dampVelocity;
        
        private void Start()
        {
            min += transform.position;
            max += transform.position;

            player = PlayerController.Instance.transform;
        }

        private void Update()
        {
            if (!player)
                return;

            intendedPosition = (player.position + playerOffset).Clamp(min, max);
            transform.position = Vector3.SmoothDamp(transform.position, intendedPosition, ref dampVelocity, .25F);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 min = this.min;
            Vector3 max = this.max;
            
            if(!UnityEditor.EditorApplication.isPlaying)
            {
                min += transform.position;
                max += transform.position;
            }
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube((max + min) / 2F, max - min);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(min, 0.25F);
            Gizmos.DrawWireSphere(max, 0.25F);
        }
        #endif
    }
}
