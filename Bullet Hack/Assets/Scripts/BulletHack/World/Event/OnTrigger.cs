using UnityEngine;

namespace BulletHack.World.Event
{
    public class OnTrigger : EventCallback
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                Execute();
        }
    }
}