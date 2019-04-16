using UnityEngine;

namespace BulletHack.World.Enemy
{
    public class EnemySweep : MonoBehaviour
    {
        public bool continuous;
        
        [ConditionalHide(nameof(continuous), true, true)]
        public float minAngle;
        [ConditionalHide(nameof(continuous), true, true)]
        public float maxAngle;

        public float speed = 5F;
        
        public int direction = 1;
        private float angle;

        private void Awake()
        {
            angle = direction > 0 ? minAngle : maxAngle;
        }

        private void Update()
        {
            angle += speed * Time.deltaTime * Mathf.Sign(direction);

            if (!continuous && (angle > maxAngle || angle < minAngle))
            {
                direction = angle > maxAngle ? -1 : 1;
                angle = Mathf.Clamp(angle, minAngle, maxAngle);
            }
            
            transform.eulerAngles = transform.eulerAngles.Set(angle, Utility.Axis.Y);
        }
    }
}
