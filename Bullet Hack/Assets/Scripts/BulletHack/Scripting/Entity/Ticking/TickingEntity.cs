using UnityEngine;

namespace BulletHack.Scripting.Entity.Ticking
{
    public abstract class TickingEntity : MonoBehaviour
    {
        [HideInInspector]
        public float tweenSpeed;

        private void Awake()
        {
            if (RegisterOnAwake())
                CombatManager.Instance.Script.AddTickingEntity(this, BoundsAware());
        }

        public abstract void Tick();
        public virtual bool BoundsAware() => true;
        public virtual bool RegisterOnAwake() => true;
    }
}