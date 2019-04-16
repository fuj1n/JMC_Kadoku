﻿using UnityEngine;

namespace BulletHack.World.Enemy.BattleEntry
{
    public class ConeBattleEntry : BattleEntryBase
    {
        public bool disableColliderOnEnter = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Vector3 heading = other.transform.position - transform.position;
                float distance = heading.magnitude;
                Vector3 direction = heading / distance;

                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, ~LayerMask.GetMask("Ignore Raycast"), QueryTriggerInteraction.Ignore))
                    if (!hit.transform.CompareTag("Player"))
                        return;

                if (disableColliderOnEnter)
                    foreach (Collider c in GetComponents<Collider>())
                        if (c.isTrigger)
                            c.enabled = false;
                OnEntry();
            }
        }
    }
}