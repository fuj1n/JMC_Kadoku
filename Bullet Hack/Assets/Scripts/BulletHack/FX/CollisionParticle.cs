using UnityEngine;

namespace BulletHack.FX
{
    [RequireComponent(typeof(Collider))]
    public class CollisionParticle : MonoBehaviour
    {
        public ParticleSystem effectTemplate;
        public Vector3 axis;
        
        private ParticleSystem effectInstance;

        private new Collider collider;
        private Transform player;
        
        private void Awake()
        {
            if (!effectTemplate)
            {
                Debug.LogError("Effect template not specified for wall " + name);
                Destroy(this); // Remove only the script
                return;
            }

            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (!player)
            {
                Debug.LogError("No player found");
                Destroy(this);
                return;
            }
            
            effectInstance = Instantiate(effectTemplate, transform);
            effectInstance.transform.localEulerAngles = axis;
            
            collider = GetComponent<Collider>();
            if (!collider)
                collider = gameObject.AddComponent<BoxCollider>();
        }

        private void Update()
        {
            Vector3 closest = collider.ClosestPoint(player.position);

            effectInstance.transform.position = closest;

            ParticleSystem.ColorBySpeedModule mod = effectInstance.colorBySpeed;

            Color c = Color.white;
            c.a = Mathf.Lerp(1F, 0F, Vector3.Distance(closest, player.position) / 2F);
            mod.color = c;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0F, .5F, .5F, .1F);
            
            BoxCollider col = GetComponent<BoxCollider>();
            if (!col || !col.enabled)
                return;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            Gizmos.DrawCube(col.center, col.size);
        }
    }
}
