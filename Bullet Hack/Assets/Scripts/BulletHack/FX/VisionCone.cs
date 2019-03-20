using System;
using System.Linq;
using Plugins.MeshBuilder;
using UnityEngine;

namespace BulletHack.FX
{
    public class VisionCone : MonoBehaviour
    {
        public float angle = 45F;
        public float range = 1.3F;

        [Header("Render")]
        public Material material;
        public float height = .25F;
        public int edgeCount = 9;

        private Mesh mesh;
        private Vector3[] vertSnapshot;

        private void Awake()
        {
            mesh = BuildMesh();
            gameObject.AddComponent<MeshFilter>().mesh = mesh;
            gameObject.AddComponent<MeshRenderer>().material = material;

            MeshCollider col = gameObject.AddComponent<MeshCollider>();
            col.convex = true;
            col.isTrigger = true;
            
            vertSnapshot = mesh.vertices;
        }

        private void LateUpdate()
        {
            Vector3[] verts = mesh.vertices;

            Vector3 indexVector = Vector3.zero;

            for (int i = 0; i < verts.Length; i++)
            {
                if (verts[i].Remove(Utility.Axis.Y).magnitude <= Mathf.Epsilon)
                {
                    indexVector = verts[i];
                    continue;
                }

                Vector3 index = transform.TransformPoint(indexVector);
                Vector3 vert = transform.TransformPoint(vertSnapshot[i]);

                Vector3 heading = vert - index;
                float distance = heading.magnitude;
                Vector3 direction = heading / distance;

                if (Physics.Raycast(index, direction, out RaycastHit hit, distance, ~LayerMask.GetMask("Ignore Raycast"), QueryTriggerInteraction.Ignore))
                {
//                    Debug.DrawLine(index, index + direction * hit.distance);
                    verts[i] = indexVector + hit.distance / transform.lossyScale.z * Vector3.Normalize(vertSnapshot[i] - indexVector);
                }
                else
                    verts[i] = vertSnapshot[i];
            }

            mesh.vertices = verts;
        }

        private Mesh BuildMesh()
        {
            float angle = this.angle * Mathf.Deg2Rad;
            float startAngle = (90F - this.angle / 2F) * Mathf.Deg2Rad;

            MeshBuilder builder = MeshBuilder.Create();

            Vector3[][] verts = new Vector3[2][];
            for (int i = 0; i < verts.Length; i++)
            {
                verts[i] = new Vector3[edgeCount + 1];
                verts[i][0] = new Vector3(0F, height * i, 0F);
            }

            float theta = angle / (edgeCount - 1F);
            float tanFactor = Mathf.Tan(theta);
            float radFactor = Mathf.Cos(theta);

            float sx = range * Mathf.Cos(startAngle);
            float sy = range * Mathf.Sin(startAngle);

            for (int seg = 1; seg <= edgeCount; seg++)
            {
                verts[0][seg] = new Vector3(sx, 0, sy);
                verts[1][seg] = new Vector3(sx, height, sy);


                if (seg > 1)
                {
                    builder.CreateFace(new[] {verts[0][0], verts[0][seg - 1], verts[0][seg]});
                    builder.CreateFace(new[] {verts[1][0], verts[1][seg - 1], verts[1][seg]}).GetFace().Reverse();
                }

                sx += -sy * tanFactor;
                sy += sx * tanFactor;

                sx *= radFactor;
                sy *= radFactor;
            }

            Mesh m = builder.Build();
            m.MarkDynamic();
            m.name = "Dynamic Cone";
            return m;
        }

#if UNITY_EDITOR
        private Mesh cachedMesh;

        private void OnValidate()
        {
            if (edgeCount < 3)
            {
                edgeCount = 3;
                Debug.LogWarning("Cannot have less than 3 segments");
            }

            if (cachedMesh)
                DestroyImmediate(cachedMesh);

            cachedMesh = BuildMesh();
        }

        private void OnDrawGizmos()
        {
            if (UnityEditor.EditorApplication.isPlaying)
                return;
            
            if (!cachedMesh)
                return;

            if (material)
                material.SetPass(0);
            Graphics.DrawMeshNow(cachedMesh, transform.localToWorldMatrix, 0);
        }
#endif
    }
}