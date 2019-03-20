using UnityEngine;

namespace BulletHack.World.BattleEntry
{
    public abstract class BattleEntryBase : MonoBehaviour
    {
        public static GameObject lastEnemyTrigger;

        public BattleEntryPoint EntryPoint { get; private set; }

        private void Awake()
        {
            EntryPoint = GetComponentInParent<BattleEntryPoint>();
        }

        protected virtual void OnEntry()
        {
            EntryPoint.EnterBattle();
            lastEnemyTrigger = EntryPoint.gameObject;
        }
    }
}