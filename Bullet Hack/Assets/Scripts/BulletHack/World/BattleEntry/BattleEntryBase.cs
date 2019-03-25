using UnityEngine;

namespace BulletHack.World.BattleEntry
{
    public abstract class BattleEntryBase : MonoBehaviour
    {
        public static BattleFinishedCallback onBattleFinish;
        public delegate void BattleFinishedCallback();

        public BattleEntryPoint EntryPoint { get; private set; }

        private void Awake()
        {
            EntryPoint = GetComponentInParent<BattleEntryPoint>();
        }

        protected virtual void OnEntry()
        {
            onBattleFinish = OnBattleFinished;
            EntryPoint.EnterBattle();
        }

        private void OnBattleFinished()
        {
            if (GameData.Instance.playerHealth > 0)
                EntryPoint.SendMessage("OnEnemyDefeated", SendMessageOptions.DontRequireReceiver);
            
            Destroy(EntryPoint.gameObject);
        }
    }
}