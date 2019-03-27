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

            if (playerHealth <= 0)
            {
                if (playerMaxHealth <= 0)
                {
                    Debug.LogError("Player max health is set to 0, the player is instead immortal to avoid infinite loops");
                    return;
                }

                playerHealth = int.MaxValue;
                Invoke(nameof(Die), 1F);
            }
        }

        private void Die()
        {
            Destroy(gameObject);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}