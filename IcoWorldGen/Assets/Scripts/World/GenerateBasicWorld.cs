using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GenerateBasicWorld
{
    private static List<Cell> cellList;
    private static  List<CellPoint> points;

    private static List<Vector3> vectors;
    private static List<int> indices;

    public static void Generate(int detailLevel)
    {
        //Get Number of Cells
        int cellNr = 20;

        if (detailLevel > 0)
        {
            for (int i = 0; i <= detailLevel; i++)
            {
                cellNr = cellNr* 4;
            }
        }

        // Instanciate Clean Arrays
        World.Cells = new Cell[cellNr];
        World.CellPoints = new CellPoint[cellNr * 3];

        World.CellColor = new Color[cellNr];
        World.CellTecID = new UInt16[cellNr];
        World.Height = new float[cellNr];

        cellList = new List<Cell>();
        points = new List<CellPoint>();


        Icosahedron();

        for (var i = 0; i < detailLevel; i++)
            Subdivide( true);

        /// normalize vectors to "inflate" the icosahedron into a sphere.
        for (var i = 0; i < vectors.Count; i++)
        {
            vectors[i] = Vector3.Normalize(vectors[i]);
            points.Add(new CellPoint(vectors[i]));
        }

       
        //Generate Cells

        int id = 0;
        for (int i = 0; i < indices.Count; i++)
        {
            if (i % 3 == 0)
            {
                id = i / 3;
                cellList.Add(new Cell((uint)id));
                //Debug.Log("Cell " + id + " added");
            }

            cellList[id].Vertexes.Add((uint)indices[i]);
            points[indices[i]].Cells.Add((uint)id);
            
        }

        // Center Cells
        foreach (Cell c in cellList)
        {
            Vector3 vec = Vector3.zero;
            foreach (int nr in c.Vertexes)
            {
                vec += points[nr].Vector;
            }
            c.Location = vec / 3;
            //Debug.Log("Cell ID " + c.Id + " centered at " + (vec / 3).ToString());
        }
        World.Cells = cellList.ToArray();
        World.CellPoints = points.ToArray();

        CheckConnections();

        TextureBuilder.NewTextures();

        //Set Scale
        SetWorldScale();
    }

    private static void SetWorldScale()
    {
        Cell c = World.Cells[0];
        Vector3 a, b;
        a = World.CellPoints[c.Vertexes[0]].Vector*World.WorldRadius;
        b = World.CellPoints[c.Vertexes[1]].Vector* World.WorldRadius;

        World.WorldCellScale = (float)Math.Sqrt(Math.Pow(a.x -b.x,2)+ Math.Pow(a.y - b.y, 2) + Math.Pow(a.z - b.z, 2));
        Debug.Log("WorldCellScale Set to: " + World.WorldCellScale);
    }

    /// <summary>
    /// create a regular icosahedron (20-sided polyhedron)
    /// </summary>
    /// <param name="primitiveType"></param>
    /// <param name="size"></param>
    /// <param name="vertices"></param>
    /// <param name="indices"></param>
    /// <remarks>
    /// You can create this programmatically instead of using the given vertex 
    /// and index list, but it's kind of a pain and rather pointless beyond a 
    /// learning exercise.
    /// </remarks>

    /// note: icosahedron definition may have come from the OpenGL red book. I don't recall where I found it. 
    private static void Icosahedron()
    {
        vectors = new List<Vector3>();
        indices = new List<int>();

        indices.AddRange(
            new int[]
            {
                0,4,1,
                0,9,4,
                9,5,4,
                4,5,8,
                4,8,1,
                8,10,1,
                8,3,10,
                5,3,8,
                5,2,3,
                2,7,3,
                7,10,3,
                7,6,10,
                7,11,6,
                11,0,6,
                0,1,6,
                6,1,10,
                9,0,11,
                9,11,2,
                9,2,5,
                7,2,11
            }
            .Select(i => i + vectors.Count)
        );

        var X = 0.525731112119133606f;
        var Z = 0.850650808352039932f;

        vectors.AddRange(
            new[]
            {
                new Vector3(-X, 0f, Z),
                new Vector3(X, 0f, Z),
                new Vector3(-X, 0f, -Z),
                new Vector3(X, 0f, -Z),
                new Vector3(0f, Z, X),
                new Vector3(0f, Z, -X),
                new Vector3(0f, -Z, X),
                new Vector3(0f, -Z, -X),
                new Vector3(Z, X, 0f),
                new Vector3(-Z, X, 0f),
                new Vector3(Z, -X, 0f),
                new Vector3(-Z, -X, 0f)
            }
        );

        indices.Reverse();

    }


    /// <remarks>
    ///      i0
    ///     /  \
    ///    m02-m01
    ///   /  \ /  \
    /// i2---m12---i1
    /// </remarks>
    /// <param name="vectors"></param>
    /// <param name="indices"></param>
    private static void Subdivide(bool removeSourceTriangles)
    {
        var midpointIndices = new Dictionary<string, int>();

        var newIndices = new List<int>(indices.Count * 4);

        if (!removeSourceTriangles)
            newIndices.AddRange(indices);

        for (var i = 0; i < indices.Count - 2; i += 3)
        {
            var i0 = indices[i];
            var i1 = indices[i + 1];
            var i2 = indices[i + 2];

            var m01 = GetMidpointIndex(midpointIndices, vectors, i0, i1);
            var m12 = GetMidpointIndex(midpointIndices, vectors, i1, i2);
            var m02 = GetMidpointIndex(midpointIndices, vectors, i2, i0);

            newIndices.AddRange(
                new[] {
                    i0,m01,m02
                    ,
                    i1,m12,m01
                    ,
                    i2,m02,m12
                    ,
                    m02,m01,m12
                }
                );

        }

        indices.Clear();
        indices.AddRange(newIndices);
    }

    private static int GetMidpointIndex(Dictionary<string, int> midpointIndices, List<Vector3> verts, int i0, int i1)
    {

        var edgeKey = string.Format("{0}_{1}", Math.Min(i0, i1), Math.Max(i0, i1));

        var midpointIndex = -1;

        if (!midpointIndices.TryGetValue(edgeKey, out midpointIndex))
        {
            var v0 = verts[i0];
            var v1 = verts[i1];

            var midpoint = (v0 + v1) / 2f;

            if (verts.Contains(midpoint))
                midpointIndex = verts.IndexOf(midpoint);
            else
            {
                midpointIndex = verts.Count;
                verts.Add(midpoint);
                midpointIndices.Add(edgeKey, midpointIndex);
            }
        }


        return midpointIndex;

    }

    private static void CheckConnections()
    {
        if (cellList != null)
        {
            List<int> contains;
            foreach (Cell c in cellList)
            {
                contains = new List<int>();

                foreach (int i1 in c.Vertexes)
                {
                    foreach (int i2 in points[i1].Cells)
                    {
                        if (i2 != c.Id)
                        {

                            if (contains.Contains(i2))
                            {
                                c.FarNeighbours.Remove((uint)i2);
                                c.CloseNeighbours.Add((uint)i2);
                            }
                            else
                            {
                                c.FarNeighbours.Add((uint)i2);
                                contains.Add(i2);
                            }
                        }
                    }
                }
            }
        }
    }
}
