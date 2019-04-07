using UIEventDelegate;
using UnityEngine;

namespace BulletHack.World.Event
{
    public abstract class EventCallback : MonoBehaviour
    {
        public ReorderableEventList onCallback;
        
        protected void Execute()
        {
            if(onCallback != null)
                EventDelegate.Execute(onCallback.List);
        }
    }
}
