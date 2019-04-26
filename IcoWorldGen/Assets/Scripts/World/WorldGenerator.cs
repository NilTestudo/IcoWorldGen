//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;



//public class WorldGenerator : MonoBehaviour
//{
    
//    private  Vector3[] directions = {
//            Vector3.left,
//            Vector3.back,
//            Vector3.right,
//            Vector3.forward
//        };

//    private int index = 0;

//    private int nrOfConnections = 0;

//    private int subdivisions = 0;

//    private float[] height;
//    private float seaLevel;

//    //public Vector3[] itterativeMotion;

//    //private int iter = 0;

//    private List<Cell> cellList;
//    private List<CellPoint> points;


//    public void GenerateWorld(int _detailLevel)
//    {
//        int cellNr = 20;

//        if (_detailLevel > 0)
//        {
//            for (int i = 0; i <= _detailLevel; i++)
//            {
//                cellNr = cellNr * 4;
//            }
//        }


//        cellList = new List<Cell>();
//        points = new List<CellPoint>();

//        var vectors = new List<Vector3>();
//        var indices = new List<int>();

//        Icosahedron(vectors, indices);

//        for (var i = 0; i < _detailLevel; i++)
//            Subdivide(vectors, indices, true);

//        /// normalize vectors to "inflate" the icosahedron into a sphere.
//        for (var i = 0; i < vectors.Count; i++)
//        {
//            vectors[i] = Vector3.Normalize(vectors[i]);
//            points.Add(new CellPoint(vectors[i]));
//        }

        
//        int id = 0;
//        for (int i = 0; i < indices.Count; i++)
//        {
//            if (i % 3 == 0)
//            {
//                id = i / 3;
//                cellList.Add(new Cell((uint)id));
//                //Debug.Log("Cell " + id + " added");
//            }

//            cellList[id].Vertexes.Add((uint)indices[i]);
//            points[indices[i]].Cells.Add((uint)id);


//        }

//        // Center Cells
//        foreach (Cell c in cellList)
//        {
//            Vector3 vec = Vector3.zero;
//            foreach (int nr in c.Vertexes)
//            {
//                vec += points[nr].Vector;
//            }
//            c.Location = vec / 3;
//            //Debug.Log("Cell ID " + c.Id + " centered at " + (vec / 3).ToString());
//        }

       

//        CheckConnections();
//        World.Cells = cellList.ToArray();
//        World.CellPoints = points.ToArray();
//        World.CellColor = new Color[cellList.Count];

//        Mesh mesh = new Mesh();
//        mesh.name = "Giahedron Sphere";
//        mesh.vertices = vectors.ToArray();
//        //mesh.uv = uvs;
//        mesh.triangles = indices.ToArray();
//        mesh.RecalculateNormals();
//        mesh.RecalculateTangents();

//        TextureBuilder.NewTextures();
//    }

//    private int GetMidpointIndex(Dictionary<string, int> midpointIndices, List<Vector3> vertices, int i0, int i1)
//    {

//        var edgeKey = string.Format("{0}_{1}", Math.Min(i0, i1), Math.Max(i0, i1));

//        var midpointIndex = -1;

//        if (!midpointIndices.TryGetValue(edgeKey, out midpointIndex))
//        {
//            var v0 = vertices[i0];
//            var v1 = vertices[i1];

//            var midpoint = (v0 + v1) / 2f;

//            if (vertices.Contains(midpoint))
//                midpointIndex = vertices.IndexOf(midpoint);
//            else
//            {
//                midpointIndex = vertices.Count;
//                vertices.Add(midpoint);
//                midpointIndices.Add(edgeKey, midpointIndex);
//            }
//        }


//        return midpointIndex;

//    }

//    /// <remarks>
//    ///      i0
//    ///     /  \
//    ///    m02-m01
//    ///   /  \ /  \
//    /// i2---m12---i1
//    /// </remarks>
//    /// <param name="vectors"></param>
//    /// <param name="indices"></param>
//    public void Subdivide(List<Vector3> vectors, List<int> indices, bool removeSourceTriangles)
//    {
//        var midpointIndices = new Dictionary<string, int>();

//        var newIndices = new List<int>(indices.Count * 4);

//        if (!removeSourceTriangles)
//            newIndices.AddRange(indices);

//        for (var i = 0; i < indices.Count - 2; i += 3)
//        {
//            var i0 = indices[i];
//            var i1 = indices[i + 1];
//            var i2 = indices[i + 2];

//            var m01 = GetMidpointIndex(midpointIndices, vectors, i0, i1);
//            var m12 = GetMidpointIndex(midpointIndices, vectors, i1, i2);
//            var m02 = GetMidpointIndex(midpointIndices, vectors, i2, i0);

//            newIndices.AddRange(
//                new[] {
//                    i0,m01,m02
//                    ,
//                    i1,m12,m01
//                    ,
//                    i2,m02,m12
//                    ,
//                    m02,m01,m12
//                }
//                );

//        }

//        indices.Clear();
//        indices.AddRange(newIndices);
//    }

//    /// <summary>
//    /// create a regular icosahedron (20-sided polyhedron)
//    /// </summary>
//    /// <param name="primitiveType"></param>
//    /// <param name="size"></param>
//    /// <param name="vertices"></param>
//    /// <param name="indices"></param>
//    /// <remarks>
//    /// You can create this programmatically instead of using the given vertex 
//    /// and index list, but it's kind of a pain and rather pointless beyond a 
//    /// learning exercise.
//    /// </remarks>

//    /// note: icosahedron definition may have come from the OpenGL red book. I don't recall where I found it. 
//    public void Icosahedron(List<Vector3> vertices, List<int> indices)
//    {

//        indices.AddRange(
//            new int[]
//            {
//                0,4,1,
//                0,9,4,
//                9,5,4,
//                4,5,8,
//                4,8,1,
//                8,10,1,
//                8,3,10,
//                5,3,8,
//                5,2,3,
//                2,7,3,
//                7,10,3,
//                7,6,10,
//                7,11,6,
//                11,0,6,
//                0,1,6,
//                6,1,10,
//                9,0,11,
//                9,11,2,
//                9,2,5,
//                7,2,11
//            }
//            .Select(i => i + vertices.Count)
//        );

//        var X = 0.525731112119133606f;
//        var Z = 0.850650808352039932f;

//        vertices.AddRange(
//            new[]
//            {
//                new Vector3(-X, 0f, Z),
//                new Vector3(X, 0f, Z),
//                new Vector3(-X, 0f, -Z),
//                new Vector3(X, 0f, -Z),
//                new Vector3(0f, Z, X),
//                new Vector3(0f, Z, -X),
//                new Vector3(0f, -Z, X),
//                new Vector3(0f, -Z, -X),
//                new Vector3(Z, X, 0f),
//                new Vector3(-Z, X, 0f),
//                new Vector3(Z, -X, 0f),
//                new Vector3(-Z, -X, 0f)
//            }
//        );

//        indices.Reverse();

//    }

//    private static int CreateVertexLine(Vector3 from, Vector3 to, int steps, int v, Vector3[] vertices)
//    {
//        for (int i = 1; i <= steps; i++)
//        {
//            vertices[v++] = Vector3.Lerp(from, to, (float)i / steps);
//        }
//        return v;
//    }

//    private static int CreateLowerStrip(int steps, int vTop, int vBottom, int t, int[] triangles)
//    {
//        for (int i = 1; i < steps; i++)
//        {
//            triangles[t++] = vBottom;
//            triangles[t++] = vTop - 1;
//            triangles[t++] = vTop;

//            triangles[t++] = vBottom++;
//            triangles[t++] = vTop++;
//            triangles[t++] = vBottom;
//        }
//        triangles[t++] = vBottom;
//        triangles[t++] = vTop - 1;
//        triangles[t++] = vTop;
//        return t;
//    }

//    private static int CreateUpperStrip(int steps, int vTop, int vBottom, int t, int[] triangles)
//    {
//        triangles[t++] = vBottom;
//        triangles[t++] = vTop - 1;
//        triangles[t++] = ++vBottom;
//        for (int i = 1; i <= steps; i++)
//        {
//            triangles[t++] = vTop - 1;
//            triangles[t++] = vTop;
//            triangles[t++] = vBottom;

//            triangles[t++] = vBottom;
//            triangles[t++] = vTop++;
//            triangles[t++] = ++vBottom;
//        }
//        return t;
//    }

//    private static void Normalize(Vector3[] vertices, Vector3[] normals)
//    {
//        for (int i = 0; i < vertices.Length; i++)
//        {
//            normals[i] = vertices[i] = vertices[i].normalized;
//        }
//    }

//    private void CreateTangents(Vector3[] vertices, Vector4[] tangents)
//    {
//        for (int i = 0; i < vertices.Length; i++)
//        {
//            Vector3 v = vertices[i];
//            v.y = 0f;
//            v = v.normalized;
//            Vector4 tangent;
//            tangent.x = -v.z;
//            tangent.y = 0f;
//            tangent.z = v.x;
//            tangent.w = -1f;
//            tangents[i] = tangent;
//        }

//        tangents[vertices.Length - 4] = tangents[0] = new Vector3(-1f, 0, -1f).normalized;
//        tangents[vertices.Length - 3] = tangents[1] = new Vector3(1f, 0f, -1f).normalized;
//        tangents[vertices.Length - 2] = tangents[2] = new Vector3(1f, 0f, 1f).normalized;
//        tangents[vertices.Length - 1] = tangents[3] = new Vector3(-1f, 0f, 1f).normalized;
//        for (int i = 0; i < 4; i++)
//        {
//            tangents[vertices.Length - 1 - i].w = tangents[i].w = -1f;
//        }
//    }

//    //public bool GenerateContinents(int continentNumber, float oceanChance, float pMovement, float pRotation)
//    //{
//    //    if(continentNumber>=World.Cells.Count())
//    //    {
//    //        return false;
//    //    }

//    //    World.Tectonics = new TectonicPlate[continentNumber];


//    //   itterativeMotion = new Vector3[World.Cells.Count()];

//    //    List<Cell> cells = World.Cells.ToList();

//    //    for (int i = 0; i < continentNumber; i++)
//    //    {
//    //        Cell c = cells[UnityEngine.Random.Range(0, cells.Count)];
//    //        bool ocean;
//    //        float height;
//    //        float chance = UnityEngine.Random.Range(0, 1);
//    //        if (chance < oceanChance)
//    //        {
//    //            ocean = true;
//    //            height = UnityEngine.Random.Range(0f, -3800 * 2);
//    //        }
//    //        else
//    //        {
//    //            ocean = false;
//    //            height = UnityEngine.Random.Range(0f, 840 * 2f);

//    //        }

//    //        World.Tectonics[i] = new TectonicPlate((UInt16)i, UnityEngine.Random.ColorHSV(0, 1, 0, 1, 1, 1), c.Id, height , ocean, pMovement, pRotation);

//    //        cells.Remove(c);
//    //    }

//    //    return true;


//    //}

//    //public static void GenerateBiomes()
//    //{
//    //    seaLevel = ((World.Instance.MaxHeight - World.Instance.MinHeight) * World.Instance.SeaLevel) + World.Instance.MinHeight;

//    //    foreach (Cell c in World.Instance.CellList)
//    //    {
//    //        float f = (c.Height - World.Instance.MinHeight) / (World.Instance.MaxHeight - World.Instance.MinHeight);

//    //        if (c.Height < seaLevel)
//    //        {
//    //            c.Color = new Color(0, 0, f);
//    //        }
//    //        else
//    //        {
//    //            c.Color = new Color(0, f, 0);
//    //        }
//    //    }
//    //}

//    //public static void GenerateElevation()
//    //{
//    //    height = new float[World.Instance.CellList.Count];
//    //    Vector3[] alternateMotion = new Vector3[cellList.Count];


//    //    World.Instance.MinHeight = World.Instance.CellList[0].Height;
//    //    World.Instance.MaxHeight = World.Instance.CellList[0].Height;

//    //    foreach (Cell c in World.Instance.CellList)
//    //    {
//    //        foreach (int i in c.CloseNeighbours)
//    //        {
//    //            float f = (Vector3.Distance(c.Location, World.Instance.CellList[i].Location) - Vector3.Distance(c.Location + itterativeMotion[c.Id], World.Instance.CellList[i].Location + itterativeMotion[i])) * 10;
//    //            c.Height += f;
//    //            Debug.Log(c.ToString() + " added height " + f);
//    //        }

//    //        if (c.Height < World.Instance.MinHeight)
//    //        {
//    //            World.Instance.MinHeight = c.Height;
//    //        }

//    //        if (c.Height > World.Instance.MaxHeight)
//    //        {
//    //            World.Instance.MaxHeight = c.Height;
//    //        }
//    //    }
//    //}

//    //public static void GenerateElevation(int iterations)
//    //{
//    //    cellList = World.Instance.CellList;

//    //    height = new float[cellList.Count];
//    //    itterativeMotion = new Vector3[cellList.Count];

//    //    for(int a = 0; a< iterations; a++)
//    //    {
//    //        for(int b = 0; b < cellList.Count; b++)
//    //        {

//    //        }
//    //    }

//    //    foreach (Cell c in World.Instance.CellList)
//    //    {
//    //        foreach (int i in c.CloseNeighbours)
//    //        {
//    //            float f = (Vector3.Distance(c.Location, World.Instance.CellList[i].Location) - Vector3.Distance(c.Location + c.PlateMovement, World.Instance.CellList[i].Location + World.Instance.CellList[i].PlateMovement)) * 10;
//    //            c.Height += f;
//    //            Debug.Log(c.ToString() + " added height " + f);
//    //        }


//    //    }
//    //}

//    //public static void GenerateErosion(int iterations)
//    //{
//    //    height = new float[World.Instance.CellList.Count];

//    //    for (int i1 = 0; i1 < iterations; i1++)
//    //    {
//    //        World.Instance.MinHeight = World.Instance.CellList[0].Height;
//    //        World.Instance.MaxHeight = World.Instance.CellList[0].Height;

//    //        for (int i2 = 0; i2 < World.Instance.CellList.Count; i2++)
//    //        {
//    //            float f = World.Instance.CellList[i2].Height;
//    //            foreach (int i in World.Instance.CellList[i2].CloseNeighbours)
//    //            {
//    //                f += World.Instance.CellList[i].Height;
//    //            }

//    //            height[i2] = World.Instance.CellList[i2].Height - (f / 4);

//    //        }

//    //        for (int i2 = 0; i2 < World.Instance.CellList.Count; i2++)
//    //        {

//    //            World.Instance.CellList[i2].Height -= height[i2];

//    //            if (World.Instance.CellList[i2].Height < World.Instance.MinHeight)
//    //            {
//    //                World.Instance.MinHeight = World.Instance.CellList[i2].Height;
//    //            }

//    //            if (World.Instance.CellList[i2].Height > World.Instance.MaxHeight)
//    //            {
//    //                World.Instance.MaxHeight = World.Instance.CellList[i2].Height;
//    //            }
//    //        }

//    //    }

//    //    //if (iter <= iterations)
//    //    //{
//    //    //    if (index == 0)
//    //    //    {
//    //    //        height = new float[World.Instance.CellList.Count];
//    //    //        World.Instance.MinHeight = World.Instance.CellList[0].Height;
//    //    //        World.Instance.MaxHeight = World.Instance.CellList[0].Height;
//    //    //    }
//    //    //    if (index <= World.Instance.CellList.Count)
//    //    //    {
//    //    //        Cell c = World.Instance.CellList[index];
//    //    //        index++;

//    //    //    }
//    //    //    else
//    //    //    {
//    //    //        index = 0;
//    //    //    }

//    //    //}
//    //    //else
//    //    //{
//    //    //    iter = 0;
//    //    //    return true;
//    //    //}
//    //    //iter++;
//    //    //return false;

//    //    //for (int i1 = 0; i1 < iterations; i1++)
//    //    //{


//    //    //    for (int i2 = 0; i2 < World.Instance.CellList.Count; i2++)
//    //    //    {
//    //    //        float f = World.Instance.CellList[i2].Height;
//    //    //        foreach (int i in World.Instance.CellList[i2].Neighbours)
//    //    //        {
//    //    //            f += World.Instance.CellList[i].Height;
//    //    //        }

//    //    //        height[i2] = World.Instance.CellList[i2].Height - (f / 4);


//    //    //    }

//    //    //    for (int i2 = 0; i2 < World.Instance.CellList.Count; i2++)
//    //    //    {

//    //    //        World.Instance.CellList[i2].Height -= height[i2];

//    //    //        if (World.Instance.CellList[i2].Height < World.Instance.MinHeight)
//    //    //        {
//    //    //            World.Instance.MinHeight = World.Instance.CellList[i2].Height;
//    //    //        }

//    //    //        if (World.Instance.CellList[i2].Height > World.Instance.MaxHeight)
//    //    //        {
//    //    //            World.Instance.MaxHeight = World.Instance.CellList[i2].Height;
//    //    //        }
//    //    //    }

//    //    //}
//    //}

   

//    private void CheckConnections()
//    {
//        if (cellList != null)
//        {
//            List<int> contains;
//            foreach (Cell c in cellList)
//            {
//                contains = new List<int>();

//                foreach (int i1 in c.Vertexes)
//                {
//                    foreach (int i2 in points[i1].Cells)
//                    {
//                        if (i2 != c.Id)
//                        {

//                            if (contains.Contains(i2))
//                            {
//                                c.FarNeighbours.Remove((uint)i2);
//                                c.CloseNeighbours.Add((uint)i2);
//                            }
//                            else
//                            {
//                                c.FarNeighbours.Add((uint)i2);
//                                contains.Add(i2);
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }

//    private int CompareVertices(List<Vector3> vec1, List<Vector3> vec2)
//    {
//        List<Vector3> shared = new List<Vector3>();
//        foreach (Vector3 nr1 in vec1)
//        {
//            if (vec2.Contains(nr1) && !shared.Contains(nr1))
//            {
//                shared.Add(nr1);
//            }

//        }
//        return shared.Count;

//    }
    

//}

