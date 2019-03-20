using System;
using UnityEngine;

namespace BulletHack.Scripting.Entity.Ticking
{
    public class Powerup : TickingEntity
    {
        public PowerupType type;
        public int turns = 3;

        private void Start()
        {
            CombatManager.Instance.Script.AddTickingEntity(this, BoundsAware());
        }

        public override void Tick()
        {
            turns--;

            if (turns <= 0)
                Destroy(gameObject);
        }

        public override bool RegisterOnAwake() => false;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Character"))
                return;

            ScriptableCharacter character = other.GetComponent<ScriptableCharacter>();

            if (!character)
                return;

            switch (type)
            {
                case PowerupType.Health:
                    character.powerups.health++;
                    break;
                case PowerupType.Shield:
                    character.powerups.shield++;
                    break;
                case PowerupType.Spread:
                    character.powerups.spread++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Destroy(gameObject);
        }

        [Serializable]
        public enum PowerupType
        {
            Health,
            Shield,
            Spread
        }
    }
}