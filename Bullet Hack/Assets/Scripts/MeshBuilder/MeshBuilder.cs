using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO: normals and uvs

namespace Plugins.MeshBuilder
{
    /// <summary>
    /// The mesh builder is a class for generating Unity meshes in a much nicer way than manually assigning all the verticies, tris and uvs
    /// Instead, the mesh builder handles all that in the background and even enables the use of non-triangular shapes which then get
    /// triangulated on build.
    /// 
    /// An instance of this class can be created using Create, this is done to make inlining of this class nicer as all the modifier functions 
    /// of this class also return the instance.
    /// </summary>
    /// <remarks>
    /// Please avoid too many verticies with the triangulator, as it can be very slow in crazy situations, avoid using more than 100 verts on a single face
    /// 
    /// The build triangulator is very simple, so it won't produce the most amazing results for n-gons, but it'll do as well as you'd expect for quads
    /// 
    /// Note that this class is not an inspector script and therefore needs a wrapper if you want to interact with it through the inspector
    /// </remarks>
    public class MeshBuilder
    {
        private Dictionary<string, Submesh> subs = new Dictionary<string, Submesh>();

        private int meshPrecision;
        private Face lastCreatedFace = null;
        private float baseNormalAngle = 90F;

        private MeshBuilder()
        { }
        public static MeshBuilder Create(int meshPrecision = 3)
        {
            MeshBuilder mb = new MeshBuilder {meshPrecision = meshPrecision};

            return mb;
        }

        public static Vector3[] Map2Y(params float[] coords)
        {
            return Map2(coords, false);
        }

        public static Vector3[] Map2Z(params float[] coords)
        {
            return Map2(coords, true);
        }

        public static Vector3[] Map2(float[] coords, bool yIsZ)
        {
            if (coords.Length % 2 != 0)
            {
                throw new ArgumentException("Coords must be in groups of 2");
            }

            Vector3[] output = new Vector3[coords.Length / 2];

            for (int i = 0; i < coords.Length; i += 2)
            {
                output[i / 2] = new Vector3(coords[i], yIsZ ? .0F : coords[i + 1], yIsZ ? coords[i + 1] : .0F);
            }

            return output;
        }
        public static Vector3[] Map3(params float[] coords)
        {
            if (coords.Length % 3 != 0)
            {
                throw new ArgumentException("Coords must be in groups of 3");
            }

            Vector3[] output = new Vector3[coords.Length / 3];

            for (int i = 0; i < coords.Length; i += 3)
            {
                output[i / 3] = new Vector3(coords[i], coords[i + 1], coords[i + 2]);
            }

            return output;
        }
        public MeshBuilder CreateFace(Vector3[] verticies, string submesh = "Default")
        {
            Submesh mesh = GetSubmesh(submesh);
            List<Guid> vertexIds = new List<Guid>();
            foreach (Vector3 vertex in verticies)
            {
                vertexIds.Add(mesh.AddVertex(vertex));
            }

            lastCreatedFace = mesh.AddFace(vertexIds);

            return this;
        }

        public Face GetFace()
        {
            return lastCreatedFace;
        }
    
        /// <summary>
        /// Removes all the stray verticies in the mesh
        /// </summary>
        /// <returns>Current instance of mesh builder</returns>
        public MeshBuilder Clean()
        {
            foreach(KeyValuePair<string, Submesh> kvp in subs)
            {
                kvp.Value.Clean();
            }

            return this;
        }
        public Submesh GetSubmesh(string name = "Default")
        {
            if (subs.ContainsKey(name))
            {
                return subs[name];
            }

            subs.Add(name, new Submesh(this));
            return subs[name];
        }

        /// <summary>
        /// Builds the mesh following triangulation
        /// </summary>
        /// <remarks>
        /// Please note that this will run <see cref="Clean()"/>, meaning that all the unused verticies will be removed
        /// </remarks>
        /// <returns>The built mesh</returns>
        public Mesh Build()
        {
            Clean();

            List<Vector3> verticies = new List<Vector3>();
            List<int> triangles = new List<int>();

            foreach (KeyValuePair<string, Submesh> kvp in subs)
            {
                Submesh mesh = kvp.Value.GetTriangulated();
                Dictionary<Guid, int> vertexMap = new Dictionary<Guid, int>();

                foreach (KeyValuePair<Guid, Vector3> vertex in mesh.verticies)
                {
                    vertexMap.Add(vertex.Key, verticies.Count);
                    verticies.Add(vertex.Value);
                }

                foreach (Face face in mesh.faces)
                {
                    foreach (Guid vertex in face.verticies)
                    {
                        if (!vertexMap.ContainsKey(vertex))
                        {
                            Debug.LogError("Vertex associated with face not found, skipping, this may cause mesh to fail building");
                        }

                        triangles.Add(vertexMap[vertex]);
                    }
                }
            }

            Mesh output = new Mesh();

            output.SetVertices(verticies);
            output.SetTriangles(triangles, 0);
            output.RecalculateNormals();

            return output;
        }

        private List<Vector3> _CalculateNormals(Submesh mesh, Dictionary<Guid, int> vertexMap)
        {
            List<Vector3> output = new List<Vector3>();

            // TODO:
            // With Unity mesh verts are repeated for every separate set of normals
            // This is super annoying
            // Have the script duplicate verticies when they need different normals
            // Just kill me xD, that's a terrible way of doing it Unity Dev Team!

            return output;
        }

        public class Vertex
        {
            private MeshBuilder parent;

            public Guid id;
            public Vector3 vertex;

            public float normalAngle;

            public Vector3 normal
            {
                get
                {
                    return Vector3.zero;
                }
            }

            public MeshBuilder Back()
            {
                return parent;
            }
        }

        public class Face
        {
            private MeshBuilder parent;
            private Submesh mesh;

            /// <summary>
            /// An internal variable, holds vertex relationships, avoid modifying directly as it may cause undefined behaviour
            /// </summary>
            internal List<Guid> verticies;

            public int vertexCount
            {
                get
                {
                    return verticies.Count;
                }
            }

            /// <summary>
            /// Calculates face normal using Newell's Method
            /// </summary>
            public Vector3 normal
            {
                get
                {
                    Vector3 normal = Vector3.zero;

                    for(int i = 0; i < verticies.Count; i++)
                    {
                        Vector3 curr = mesh.verticies[verticies[i]];
                        Vector3 next = mesh.verticies[verticies[(i + 1) % verticies.Count]];

                        normal.Set(
                                normal.x + (curr.y - next.y) * (curr.z + next.z),   // X
                                normal.y + (curr.z - next.z) * (curr.x + next.x),   // Y
                                normal.z + (curr.x - next.x) * (curr.y + next.y));  // Z
                    }

                    return normal;
                }
            }

            public Face(MeshBuilder parent, Submesh mesh) : this(parent, mesh, new List<Guid>())
            { }

            /// <summary>
            /// Avoid creating faces manually, <see cref="CreateFace(Vector3[], string)"/>
            /// </summary>
            public Face(MeshBuilder parent, Submesh mesh, List<Guid> verticies)
            {
                this.parent = parent;
                this.verticies = verticies;
            }
            public Face Reverse()
            {
                verticies.Reverse();

                return this;
            }
            public MeshBuilder Back()
            {
                return parent;
            }
        }

        public class Submesh
        {
            public Dictionary<Guid, Vector3> verticies = new Dictionary<Guid, Vector3>();
            public List<Face> faces = new List<Face>();

            private MeshBuilder parent;
            public Submesh(MeshBuilder parent)
            {
                this.parent = parent;
            }
            public Guid GetVertexId(Vector3 vertex)
            {
                vertex = TrimVector(vertex);

                if (!verticies.ContainsValue(vertex))
                {
                    return Guid.Empty;
                }

                return verticies.First(kvp => kvp.Value.Equals(vertex)).Key;
            }
            public Guid AddVertex(Vector3 vertex)
            {
                vertex = TrimVector(vertex);

                Guid vertexId = GetVertexId(vertex);

                if (vertexId != Guid.Empty)
                {
                    return vertexId;
                }

                vertexId = Guid.NewGuid();
                verticies.Add(vertexId, vertex);
                return vertexId;
            }
            public Face[] GetVertexUses(Guid vertex)
            {
                return faces.Where(face => face.verticies.Any(vert => vert.Equals(vertex))).ToArray();
            }
            public Face AddFace(List<Guid> verticies)
            {
                if (verticies.Count < 3)
                {
                    throw new ArgumentException("A face must have at least 3 verticies, got " + verticies.Count);
                }

                if (faces.Any(f => f.verticies.OrderBy(vert => vert).SequenceEqual(verticies)))
                {
                    return faces.Single(f => f.verticies.OrderBy(vert => vert).SequenceEqual(verticies));
                }

                Face face = new Face(parent, this, verticies);
                faces.Add(face);
                return face;
            }
            public void Clean()
            {
                List<Guid> pendingRemoval = verticies.Where(kvp => GetVertexUses(kvp.Key).Length == 0).Select(kvp => kvp.Key).ToList();

                foreach(Guid id in pendingRemoval)
                {
                    verticies.Remove(id);
                }
            }
            public Submesh GetTriangulated()
            {
                Submesh mesh = new Submesh(parent) {verticies = new Dictionary<Guid, Vector3>(verticies)};

                foreach (Face face in faces)
                {
                    if (face.vertexCount < 3)
                    {
                        Debug.LogError("Found face with less than three verticies, skipping...");
                        continue;
                    }

                    TriangulateRecurse(face, mesh.faces);
                }

                return mesh;
            }
            public MeshBuilder Back()
            {
                return parent;
            }
            private void TriangulateRecurse(Face face, List<Face> faces)
            {
                if (face.vertexCount == 3)
                {
                    faces.Add(face);
                    return;
                }

                Face f1 = new Face(parent, this);
                Face f2 = new Face(parent, this);

                for (int i = 0; i <= Mathf.CeilToInt(face.vertexCount / 2F); i++)
                {
                    f1.verticies.Add(face.verticies[i]);
                }

                for (int i = Mathf.CeilToInt(face.vertexCount / 2F); i < face.vertexCount; i++)
                {
                    f2.verticies.Add(face.verticies[i]);
                }

                f2.verticies.Add(face.verticies[0]);

                TriangulateRecurse(f1, faces);
                TriangulateRecurse(f2, faces);
            }
            private Vector3 TrimVector(Vector3 vector)
            {
                return new Vector3((float)Math.Round(vector[0], parent.meshPrecision), (float)Math.Round(vector[1], parent.meshPrecision), (float)Math.Round(vector[2], parent.meshPrecision));
            }
        }
    }
}
