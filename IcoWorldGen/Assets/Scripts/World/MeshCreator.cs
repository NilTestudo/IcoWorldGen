using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshCreator
{


    public static Mesh WorldMesh()
    {
        Debug.Log("Creating Mesh");
        int scale = World.WorldRadius;
        var vectors = new List<Vector3>();
        var indices = new List<int>();

        int index = 0;
        foreach (Cell c in World.Cells)
        {
            foreach (int v3 in c.Vertexes)
            {
                vectors.Add(World.CellPoints[v3].Vector * scale);
            }
            //vectors.AddRange(c.Vertexes);
            indices.Add(index);
            indices.Add(index + 1);
            indices.Add(index + 2);
            
            index += 3;
        }

        // Create uvs
        Vector2[] uvs = new Vector2[vectors.Count];

        int size = (int)Math.Ceiling(Math.Sqrt(World.Cells.Length));
        
        float offset = 1f/size;
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(0, offset);
        uvs[2] = new Vector2(offset, offset);
        Debug.Log("UV offset is: " + offset);
        Debug.Log("UV's created for CellId: 0; " + uvs[0] + " " + uvs[1] + " " + uvs[2]);


        int nr = 1;

        for (var i = 3; i < uvs.Length - 2; i += 3)
        {
            // From TextureBuilder.cs
            //xa = (int)(Math.Floor((double)(i / size))) * scale;
            //xb = ((int)(Math.Floor((double)(i / size))) + 1) * scale - 1;
            //ya = (int)(Math.Floor((double)(i % size))) * scale;
            //yb = ((int)(Math.Floor((double)(i % size))) + 1) * scale - 1;

            // Reverse this!
            //uvs[i] = new Vector2(
            //   (float)((Math.Floor((double)(nr / size))) * offset),
            //   (float)(Math.Floor((double)(nr % size)) * offset)
            //   );
            //uvs[i + 1] = new Vector2(
            //    (float)((Math.Floor((double)(nr / size))) * offset),
            //    (float)(Math.Floor((double)(nr % size) + 1) * offset)
            //    );
            //uvs[i + 2] = new Vector2(
            //    (float)((Math.Floor((double)(nr / size)) + 1) * offset),
            //    (float)(Math.Floor((double)(nr % size) + 1) * offset)
            //    );


            uvs[i] = new Vector2(
                (float)(Math.Floor((double)(nr % size)) * offset),
                (float)((Math.Floor((double)(nr / size))) * offset)
                );
            uvs[i + 1] = new Vector2(
                (float)(Math.Floor((double)(nr % size) + 1) * offset),
                (float)((Math.Floor((double)(nr / size))) * offset)
                );
            uvs[i + 2] = new Vector2(
                (float)(Math.Floor((double)(nr % size) + 1) * offset),
                (float)((Math.Floor((double)(nr / size))+1) * offset)
                );

            //Debug.Log("UV's created for CellId: " + nr + "; " + uvs[i] + " " + uvs[i + 1] + " " + uvs[i + 2]);

            nr++;
        }


    
        //    for (int i = 0; i < uvs.Length; i++)
        //{
        //    uvs[i] = new Vector2(vectors[i].x, vectors[i].z);
        //}

        Mesh mesh = new Mesh();
        mesh.name = "Giahedron Sphere";
        mesh.vertices = vectors.ToArray();
        mesh.uv = uvs;
        mesh.triangles = indices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        Debug.Log("Mesh Created");
        return mesh;
    }
}
