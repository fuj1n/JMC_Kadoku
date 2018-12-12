using UnityEngine;

public abstract class TickingEntity : MonoBehaviour
{
    [HideInInspector]
    public float tweenSpeed;

    private void Awake()
    {
        CombatManager.Instance.Script.AddTickingEntity(this);
    }

    public abstract void Tick();
}
