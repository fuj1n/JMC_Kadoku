using UnityEngine;

public abstract class TickingEntity : MonoBehaviour
{
    [HideInInspector]
    public float tweenSpeed;

    private void Awake()
    {
        CombatManager.Instance.Script.AddTickingEntity(this, BoundsAware());
    }

    public abstract void Tick();
    public virtual bool BoundsAware() => true;
}
