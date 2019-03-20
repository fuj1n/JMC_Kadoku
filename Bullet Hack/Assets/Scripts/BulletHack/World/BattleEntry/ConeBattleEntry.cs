using UnityEngine;

namespace BulletHack.World.BattleEntry
{
    public class ConeBattleEntry : BattleEntryBase
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Vector3 heading = other.transform.position - transform.position;
                float distance = heading.magnitude;
                Vector3 direction = heading / distance;
                
                if(Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, ~LayerMask.GetMask("Ignore Raycast"), QueryTriggerInteraction.Ignore))
                    if (!hit.transform.CompareTag("Player"))
                        return;
                
                OnEntry();
            }
        }
    }
}