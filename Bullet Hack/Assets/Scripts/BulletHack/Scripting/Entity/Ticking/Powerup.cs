using System;
using UnityEngine;

namespace BulletHack.Scripting.Entity.Ticking
{
    public class Powerup : TickingEntity
    {
        public PowerupType type;
        public int turns = 3;

        public AudioClip pickupSound;

        private void Start()
        {
            if(CombatManager.Instance)
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
            if (!other.CompareTag("Character") && !other.CompareTag("Player"))
                return;

            if (!GameData.Instance)
                return;
            
            GameData.Instance.powerups[type]++;
            
            if(pickupSound)
                SoundManager.PlayClip(pickupSound, SoundManager.Channel.SoundEffect);
            
            Destroy(gameObject);
        }

        public enum PowerupType
        {
            Health,
            Shield,
            Spread
        }
    }
}