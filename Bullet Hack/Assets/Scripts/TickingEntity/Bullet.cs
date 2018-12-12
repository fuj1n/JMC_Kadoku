using DG.Tweening;
using UnityEngine;

public class Bullet : TickingEntity
{
    public Transform target;

    public override void Tick()
    {
        transform.DOMove(transform.position + transform.forward * 2, tweenSpeed / 2F);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            ScriptableCharacter character = other.GetComponent<ScriptableCharacter>();
            if (character)
                character.health--;

            Destroy(gameObject);
        }
    }
}
