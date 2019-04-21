using UnityEngine;

namespace BulletHack.World.Event
{
    public class OnUntrigger : EventCallback
    {
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                Execute();
        }
    }
}