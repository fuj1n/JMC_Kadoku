using DG.Tweening;
using UnityEngine;

namespace BulletHack.Scripting.Entity.Ticking
{
    public class Bullet : TickingEntity
    {
        public GameObject hitParticles;

        [HideInInspector]
        public Transform target;

        private Transform render;

        private void Start()
        {
            render = transform.GetChild(0);
        }

        private void Update()
        {
            render.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }

        public override void Tick()
        {
            transform.DOMove(transform.position + transform.forward * 2, tweenSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == target)
            {
                ScriptableCharacter character = other.GetComponent<ScriptableCharacter>();
                if (character)
                    character.health--;

                if (hitParticles)
                {
                    GameObject particles = Instantiate(hitParticles);
                    particles.transform.position = transform.position;
                    particles.transform.forward = transform.forward;
                }

                Destroy(gameObject);
            }
        }
    }
}