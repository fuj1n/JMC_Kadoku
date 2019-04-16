using System;
using UnityEngine;

namespace BulletHack.World
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }
        
        public float movementSpeed = 25F;

        [Header("Animation")]
        public float tiltAmount = 45F;
        public float tiltDamping = .25F;
        public float rotationDamping = .2F;

        [Header("Particles")]
        public ParticleSystem[] particles;
        public float particleMinSpeed;
        public float particleMaxSpeed = 7.5F;

        private float yDampVelocity;
        private float xDampVelocity;

        private CharacterController controller;
        private float yVelocity;

        [Header("Spawn Effect")]
        public GameObject[] showObjects;
        public float spawnTime = 1F;

        private void Awake()
        {
            Instance = this;
            
            controller = GetComponent<CharacterController>();
            
            if(spawnTime > 0F)
                foreach(GameObject ob in showObjects)
                    ob.SetActive(false);
        }

        private void Update()
        {
            if (spawnTime > 0)
            {
                spawnTime -= Time.deltaTime;

                if (spawnTime <= 0)
                    foreach (GameObject ob in showObjects)
                        ob.SetActive(true);
                else
                    return;
            }
            
            Quaternion camera = Quaternion.Euler(CameraSystem.Instance.GetActiveCamera().transform.eulerAngles.Isolate(Utility.Axis.Y));

            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0F, Input.GetAxisRaw("Vertical"));
            movement = camera * movement;

            if (spawnTime > 0 || Time.timeScale < Mathf.Epsilon)
                movement *= 0F;

            if (movement.magnitude > Mathf.Epsilon)
            {
                float angle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref yDampVelocity, rotationDamping);
                Vector3 newRot = transform.eulerAngles.Set(angle, Utility.Axis.Y);

                transform.eulerAngles = newRot;
            }

            float tilt = Mathf.SmoothDampAngle(transform.eulerAngles.x, tiltAmount * movement.magnitude, ref xDampVelocity, tiltDamping);
            transform.eulerAngles = transform.eulerAngles.Set(tilt, Utility.Axis.X);

            yVelocity -= 9.81F * Time.deltaTime;
            controller.Move((movement * movementSpeed + Vector3.up * yVelocity) * Time.deltaTime);

            foreach (ParticleSystem particle in particles)
            {
                ParticleSystem.VelocityOverLifetimeModule mod = particle.velocityOverLifetime;
                mod.z = Mathf.Lerp(particleMinSpeed, particleMaxSpeed, controller.velocity.magnitude);
            }
            
            if (controller.isGrounded)
                yVelocity = 0F;
        }
    }
}