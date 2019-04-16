using System;
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
            
            Destroy(gameObject);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        public void ResetDead()
        {
            isDead = false;
            playerHealth = playerMaxHealth;
        }

        public interface IDeathHandler
        {
            void OnDeath();
        }
    }
}