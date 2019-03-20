using System;
using UnityEngine;

namespace Plugins.MeshBuilder
{
    public static class PolygonBuilder
    {

        //TODO: build actually usable UVs
        //TODO: figure out normals (preferably with configurable smoothness)
        public static Mesh Generate(int sides, float inRadius, float extrude = 0F, bool isOutline = false, float outlineWidth = .25F)
        {
            if (sides < 3)
            {
                throw new ArgumentException("Cannot generate a mesh with less than three sides");
            }

            float outRadius = inRadius * (1 / Mathf.Cos(Mathf.PI / sides));

            MeshBuilder builder = MeshBuilder.Create();

            Vector3[] sideVerts = new Vector3[sides];
            Vector3[] extrudeVerts = new Vector3[sides];

            for (int i = 0; i < sides; i++)
            {
                sideVerts[i] = new Vector3(outRadius * Mathf.Cos(2 * Mathf.PI * i / sides), 0F, outRadius * Mathf.Sin(2 * Mathf.PI * i / sides));

                if (extrude > 0F)
                {
                    extrudeVerts[i] = sideVerts[i] - Vector3.down * extrude;
                }
            }

            if (!isOutline)
            {
                builder.CreateFace(sideVerts);
                if (extrude > 0F)
                {
                    builder.CreateFace(extrudeVerts).GetFace().Reverse();

                    for (int i = 0; i < sides; i++)
                    {
                        builder.CreateFace(new[] {
                                sideVerts[i], sideVerts[i+1 == sides ? 0 : i+1],
                                extrudeVerts[i+1 == sides ? 0 : i+1], extrudeVerts[i]
                        }).GetFace().Reverse();
                    }
                }
            }
            else
            {
                Vector3[] insideVerts = new Vector3[sides];
                Vector3[] extrudeInsideVerts = new Vector3[sides];

                for (int i = 0; i < sides; i++)
                {
                    insideVerts[i] = new Vector3((outRadius - outlineWidth) * Mathf.Cos(2 * Mathf.PI * i / sides), 0F, (outRadius - outlineWidth) * Mathf.Sin(2 * Mathf.PI * i / sides));

                    if (extrude > 0F)
                    {
                        extrudeInsideVerts[i] = insideVerts[i] - Vector3.down * extrude;
                    }
                }

                for (int i = 0; i < sides; i++)
                {
                    builder.CreateFace(new[]
                    {
                            sideVerts[i], sideVerts[i+1 == sides ? 0 : i+1],
                            insideVerts[i+1 == sides ? 0 : i+1], insideVerts[i]
                    });

                    if (extrude > 0F)
                    {
                        builder.CreateFace(new[]
                        {
                                extrudeVerts[i], extrudeVerts[i+1 == sides ? 0 : i+1],
                                extrudeInsideVerts[i+1 == sides ? 0 : i+1], extrudeInsideVerts[i]
                        }).GetFace().Reverse();

                        builder.CreateFace(new[]
                        {
                                sideVerts[i], sideVerts[i+1 == sides ? 0 : i+1],
                                extrudeVerts[i+1 == sides ? 0 : i+1], extrudeVerts[i]
                        }).GetFace().Reverse();

                        builder.CreateFace(new[]
                        {
                                insideVerts[i], insideVerts[i+1 == sides ? 0 : i+1],
                                extrudeInsideVerts[i+1 == sides ? 0 : i+1], extrudeInsideVerts[i]
                        });
                    }
                }
            }

            //// Dynamically build mesh with x sides and Y thickness
            //Mesh mesh = new Mesh();

            //List<Vector3> verticies = new List<Vector3>();
            //List<int> triangles = new List<int>();
            //List<Vector2> uvs = new List<Vector2>();

            //if (!isOutline)
            //{
            //    verticies.Add(Vector3.zero); // The center of the top faces
            //    uvs.Add(new Vector2((verticies[0].x + outRadius / 2) / outRadius * .75F, (verticies[0].z + outRadius / 2) / outRadius * .75F));

            //    for (int i = 0; i < sides; i++)
            //    {
            //        verticies.Add(new Vector3(outRadius * Mathf.Cos(2 * Mathf.PI * i / sides), 0F, outRadius * Mathf.Sin(2 * Mathf.PI * i / sides)));
            //        uvs.Add(new Vector2((verticies[i + 1].x + outRadius / 2) / outRadius * .75F, (verticies[i + 1].z + outRadius / 2) / outRadius * .75F));

            //        triangles.Add(i + 2 <= sides ? i + 2 : 1);
            //        triangles.Add(i + 1);
            //        triangles.Add(0);
            //    }

            //    if(extrude > 0)
            //    {
            //        for(int i = 0; i < sides; i++)
            //        {
            //            verticies.Add(verticies[i + 1] + new Vector3(0, -extrude, 0));
            //            uvs.Add(Vector2.zero);

            //            triangles.Add(sides + i + 1);
            //            triangles.Add(i + 1);
            //            triangles.Add(i + 2 <= sides ? i + 2 : 1);

            //            triangles.Add(i + 2 <= sides ? i + 2 : 1);
            //            triangles.Add(i + 2 <= sides ? sides + i + 2 : sides + 1);
            //            triangles.Add(sides + i + 1);
            //        }
            //    }
            //} else
            //{
            //    for (int i = 0; i < sides; i++)
            //    {
            //        verticies.Add(new Vector3(outRadius * Mathf.Cos(2 * Mathf.PI * i / sides), 0F, outRadius * Mathf.Sin(2 * Mathf.PI * i / sides)));
            //        verticies.Add(new Vector3((outRadius - outlineWidth) * Mathf.Cos(2 * Mathf.PI * i / sides), 0F, outRadius - (outlineWidth) * Mathf.Sin(2 * Mathf.PI * i / sides)));
            //        //TODO figure out triangles for outlines
            //        triangles.Add(i * 2);
            //        triangles.Add(i * 2 + 1);
            //        triangles.Add(i == sides - 1 ? 0 : i * 2 + 2);
            //        triangles.Add(i == sides - 1 ? 1 : i * 2 + 3);
            //        triangles.Add(i * 2 + 1);
            //        triangles.Add(i == sides - 1 ? 0 : i * 2 + 2);
            //    }
            //}

            //mesh.SetVertices(verticies);
            //mesh.SetTriangles(triangles, 0, true, 0);
            //mesh.SetUVs(0, uvs);

            //mesh.RecalculateNormals();

            return builder.Build();
        }
    }
}
