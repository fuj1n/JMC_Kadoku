using UnityEngine;

namespace BulletHack.FX
{
    public class RotateEffect : MonoBehaviour
    {
        public Vector3 amount = new Vector3(45F, 35F, 25F);
        public float multiplier = 1F;

        private void Update() =>
                transform.Rotate(amount * multiplier * Time.deltaTime);
    }
}