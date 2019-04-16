using UnityEngine;

namespace BulletHack.World.Tutorial
{
    public class TutorialDeathHandler : MonoBehaviour, GameData.IDeathHandler
    {
        public static TutorialDeathHandler Instance { get; private set; }

        [HideInInspector]
        public Vector3 lastCheckpoint;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameData.Instance.customDeathHandler = this;

            lastCheckpoint = PlayerController.Instance.transform.position;
        }

        public void OnDeath()
        {
            PlayerController.Instance.transform.position = lastCheckpoint;
            
            GameData.Instance.Invoke(nameof(GameData.ResetDead), 3F);
        }
    }
}