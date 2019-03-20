using System;
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
        
        private void Awake()
        {
            mesh = BuildMesh();
            gameObject.AddComponent<MeshFilter>().mesh = mesh;
            gameObject.AddComponent<MeshRenderer>().material = material;
            
            MeshCollider col = gameObject.AddComponent<MeshCollider>();
            col.convex = true;
            col.isTrigger = true;
        }

        private Mesh BuildMesh()
        {
            float angle = this.angle * Mathf.Deg2Rad;
            float startAngle = (90F - this.angle / 2F) * Mathf.Deg2Rad;
            
            MeshBuilder builder = MeshBuilder.Create();

            Vector3[][] verts = new Vector3[2][];
            for (int i = 0; i < 2; i++)
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

                sx += -sy * tanFactor;
                sy += sx * tanFactor;

                sx *= radFactor;
                sy *= radFactor;
            }
            
            builder.CreateFace(verts[0]);
            
            if (height < 0)
                builder.GetFace().Reverse();
            
            builder.CreateFace(verts[1]);

            if (height >= 0)
                builder.GetFace().Reverse();

            for (int i = 0; i < edgeCount + 1; i++)
            {
                builder.CreateFace(new[] {
                        verts[0][i], verts[0][i+1 == edgeCount + 1 ? 0 : i+1],
                        verts[1][i+1 == edgeCount + 1 ? 0 : i+1], verts[1][i]
                }).GetFace().Reverse();
            }
            
            return builder.Build();
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
            if (!cachedMesh)
                return;

            if(material)
                material.SetPass(0);
            Graphics.DrawMeshNow(cachedMesh, transform.localToWorldMatrix, 0);
        }
#endif
    }
}