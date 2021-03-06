﻿using System;
using System.Collections.Generic;
using System.Linq;
using BulletHack.Scripting.Entity.Ticking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletHack
{
    public class GameData : MonoBehaviour
    {
        public static GameData Instance { get; private set; }

        [NonSerialized]
        public int playerHealth;
        public int playerMaxHealth;
        [NonSerialized]
        public bool isDead;
        
        public Dictionary<Powerup.PowerupType, int> powerups = Enum.GetValues(typeof(Powerup.PowerupType))
                .Cast<Powerup.PowerupType>().ToDictionary(x => x, x => 0);

        public IDeathHandler customDeathHandler;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            playerHealth = playerMaxHealth;
        }

        private void Update()
        {
            playerHealth = Mathf.Clamp(playerHealth, 0, playerMaxHealth);

            if (isDead)
                return;
            
            if (playerHealth <= 0)
            {
                if (playerMaxHealth <= 0)
                {
                    Debug.LogError("Player max health is set to 0, the player is instead immortal to avoid infinite loops");
                    return;
                }

                isDead = true;
                Invoke(nameof(Die), 1F);
            }
        }

        private void Die()
        {
            if (customDeathHandler != null)
            {
                customDeathHandler.OnDeath();
                return;
            }
            
            DestroyActiveInstance();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        public void ResetDead()
        {
            isDead = false;
            playerHealth = playerMaxHealth;
        }

        public static void DestroyActiveInstance()
        {
            if(Instance)
                Destroy(Instance.gameObject);
        }

        public interface IDeathHandler
        {
            void OnDeath();
        }
    }
}