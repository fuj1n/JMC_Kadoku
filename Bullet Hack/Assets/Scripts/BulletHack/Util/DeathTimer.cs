using UnityEngine;

namespace BulletHack.Util
{
    public class DeathTimer : MonoBehaviour
    {
        public float time;

        private void Update()
        {
            time -= Time.deltaTime;
            if (time <= 0F)
                Destroy(gameObject);
        }
    }
}