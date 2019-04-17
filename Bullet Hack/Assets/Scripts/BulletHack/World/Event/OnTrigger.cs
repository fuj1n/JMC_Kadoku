using UnityEngine;

namespace BulletHack.World.Event
{
    public class OnTrigger : EventCallback
    {
        public bool raycast = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            Vector3 heading = other.transform.position - transform.position;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;

            if (raycast && Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, ~LayerMask.GetMask("Ignore Raycast"), QueryTriggerInteraction.Ignore))
                if (!hit.transform.CompareTag("Player"))
                    return;

            Execute();
        }
    }
}