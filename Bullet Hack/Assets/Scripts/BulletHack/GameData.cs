using System;
using UnityEngine;

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
            if(Instance)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);

            playerHealth = playerMaxHealth;
        }
    }
}
