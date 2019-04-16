using BulletHack.World.Messaging;
using UnityEngine;

namespace BulletHack.World.Tutorial
{
    public class TutorialSetCheckpoint : MonoBehaviour, ICombatEntryEvent
    {
        public Vector3 restorePoint;
        
        public bool OnPreCombatEnter()
        {
            TutorialDeathHandler.Instance.lastCheckpoint = restorePoint;
            GameData.Instance.ResetDead();
            return false;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(restorePoint, 1.5F);
        }
    }
}