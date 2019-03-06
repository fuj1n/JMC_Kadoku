using UnityEngine;

namespace BulletHack.World
{
    public class PlayerController : MonoBehaviour
    {
        public float rotationDamping = .2F;
        public float movementSpeed = 25F;
        
        private float yDampVelocity;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Quaternion camera = Quaternion.Euler(CameraSystem.Instance.GetActiveCamera().transform.eulerAngles.Isolate(Utility.Axis.Y));
            
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0F, Input.GetAxisRaw("Vertical"));
            movement = camera * movement;

            if (movement.magnitude > Mathf.Epsilon)
            {
                float angle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref yDampVelocity,  rotationDamping);
                Vector3 newRot = transform.eulerAngles.Set(angle, Utility.Axis.Y);
                
                transform.eulerAngles = newRot;
            }

            rb.velocity = movement * movementSpeed;
        }
    }
}
