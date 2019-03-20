using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace BulletHack.World.Enemy
{
    public class EnemyPath : MonoBehaviour
    {
        public Vector3[] path = { };
        public float tiltAmount = 15F;
        public float speed = 2F;
        public float waitTime = .5F;

        private int index;
        private Tween motion;

        private bool isWait;

#if UNITY_EDITOR
        // Used for gizmo drawing
        private List<Vector3> absolutePath;
#endif

        private void Awake()
        {
            Debug.Assert(path != null, "Path is null for " + name);

            System.Array.Resize(ref path, path.Length + 1);
            path[path.Length - 1] = -path.Aggregate((current, val) => current + val);

#if UNITY_EDITOR
            Vector3 curr = transform.position;
            absolutePath = path.Select(x => curr += x).ToList();
            absolutePath.Insert(0, transform.position);
#endif
        }

        private void Update()
        {
            if (motion != null && motion.IsPlaying())
                return;

            if (!isWait)
            {
                Vector3 normal = path[index].normalized;
                float angle = Mathf.Atan2(normal.x, normal.z) * Mathf.Rad2Deg;
                motion = transform.DORotate(transform.eulerAngles.Set(angle, Utility.Axis.Y), waitTime);

                isWait = true;
                return;
            }

            motion = transform.DOBlendableMoveBy(path[index], path[index].magnitude / speed);

            Sequence sq = DOTween.Sequence();
            sq.Append(transform.DOLocalRotate(transform.localEulerAngles.Set(tiltAmount, Utility.Axis.X), .25F));
            sq.AppendInterval(path[index].magnitude / speed - .5F);
            sq.Append(transform.DOLocalRotate(transform.localEulerAngles.Set(0F, Utility.Axis.X), .25F));

            motion.SetEase(Ease.InOutQuad);

            index++;
            if (index >= path.Length)
                index = 0;
            isWait = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (path == null || path.Length == 0)
                return;

            Gizmos.color = Color.red;

            if (!UnityEditor.EditorApplication.isPlaying)
            {
                Vector3 current = transform.position;
                foreach (Vector3 point in path)
                {
                    Gizmos.DrawLine(current, current + point);
                    current += point;

                    Gizmos.DrawSphere(current, .25F);
                }

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(current, current - path.Aggregate((curr, val) => curr + val));
            }
            else
            {
                for (int i = 1; i < absolutePath.Count; i++)
                    Gizmos.DrawLine(absolutePath[i - 1], absolutePath[i]);
            }
        }
#endif
    }
}