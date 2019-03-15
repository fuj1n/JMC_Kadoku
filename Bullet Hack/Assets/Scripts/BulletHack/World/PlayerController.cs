using UnityEngine;

namespace BulletHack.World
{
    public class PlayerController : MonoBehaviour
    {
        public float movementSpeed = 25F;

        [Header("Animation")]
        public float tiltAmount = 45F;
        public float tiltDamping = .25F;
        public float rotationDamping = .2F;

        [Header("Particles")]
        public ParticleSystem[] particles;
        public float particleMinSpeed = 5F;
        public float particleMaxSpeed = 10F;

        private float yDampVelocity;
        private float xDampVelocity;

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
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref yDampVelocity, rotationDamping);
                Vector3 newRot = transform.eulerAngles.Set(angle, Utility.Axis.Y);

                transform.eulerAngles = newRot;
            }

            float tilt = Mathf.SmoothDampAngle(transform.eulerAngles.x, tiltAmount * movement.magnitude, ref xDampVelocity, tiltDamping);
            transform.eulerAngles = transform.eulerAngles.Set(tilt, Utility.Axis.X);

            foreach (ParticleSystem particle in particles)
            {
                ParticleSystem.MainModule main = particle.main;
                main.startSpeed = Mathf.Lerp(particleMinSpeed, particleMaxSpeed, movement.magnitude);
            }

            rb.velocity = movement * movementSpeed;
        }
    }
}