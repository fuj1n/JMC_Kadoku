using UnityEngine;

namespace BulletHack.World.BattleEntry
{
    public class ConeBattleEntry : BattleEntryBase
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                OnEntry();
        }
    }
}
