using BulletHack.UI;
using UnityEngine;

namespace BulletHack.World.Messaging
{
    public class TutorialSafeCombatCancel : MonoBehaviour, ICombatEntryEvent
    {
        public Vector3 restorePoint;

        public bool OnPreCombatEnter()
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = restorePoint;
            PopupMessageUi.Open(new PopupMessageUi.PopupData()
            {
                    color = Color.red,
                    direction = PopupMessageUi.PopupDirection.Left,
                    text = "Avoid the enemy to proceed"
            });
            
            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(restorePoint, 1.5F);
        }
    }
}