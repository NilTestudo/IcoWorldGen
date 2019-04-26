using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public static class GenerateTectonics
{
    public static Vector3[] TectonicMotion;

    public static bool GenerateContinents(int continentNumber, float oceanChance, float pMovement, float pRotation)
    {
        if (continentNumber >= World.Cells.Count())
        {
            return false;
        }

        World.Tectonics = new TectonicPlate[continentNumber];
        World.CellTecID = new UInt16[World.Cells.Count()];
        World.Height = new float[World.Cells.Count()];
        TectonicMotion = new Vector3[World.Cells.Count()];


        List<Cell> cells = World.Cells.ToList();

        for (int i = 0; i < continentNumber; i++)
        {
            Cell c = cells[UnityEngine.Random.Range(0, cells.Count)];
            bool ocean;
            float height;
            float chance = UnityEngine.Random.Range(0, 1);
            if (chance < oceanChance)
            {
                ocean = true;
                height = UnityEngine.Random.Range(0f, -3800 * 2);
            }
            else
            {
                ocean = false;
                height = UnityEngine.Random.Range(0f, 840 * 2f);

            }

            World.Tectonics[i] = new TectonicPlate((UInt16)i, UnityEngine.Random.ColorHSV(0, 1, 0, 1, 1, 1), c.Id, height, ocean, pMovement, pRotation);

            cells.Remove(c);
        }

        return true;


    }

    public static void SetContinentsInertia(float movement, float rotation)
    {
        TectonicMotion = new Vector3[World.Cells.Count()];

        for(int i = 0; i <= World.Tectonics.Count(); i++)
        {
            TectonicPlate t = World.Tectonics[i];
            t.Movement = t.LocalMatrix.MultiplyPoint3x4(new Vector3(UnityEngine.Random.Range(-movement, movement), UnityEngine.Random.Range(-movement, movement), 0)) - t.Location;

            foreach(uint u in t.PlateCellList)
            {
                Vector3 vec = rotation * t.LocalMatrix.inverse.MultiplyPoint3x4(World.Cells[u].Location);
                TectonicMotion[u] += t.LocalMatrix.MultiplyPoint3x4(vec) - World.Cells[u].Location;
                
            }
        }
    }

    public static bool GrowContinents()
    {
        int i = 0;
        foreach (TectonicPlate con in World.Tectonics)
        {
            bool b = con.Grow();
            if (b)
            {
                i++;
            }
        }

        if (i == World.Tectonics.Count())
        {
            return true;

        }
        return false;
    }

    public static void GenerateElevation(int iterations)
    {

        float[] height = new float[World.Cells.Length];
        Vector3[] itterativeMotion = new Vector3[World.Cells.Length];

        for (int a = 0; a < iterations; a++)
        {
            for (int b = 0; b < World.Cells.Length; b++)
            {
                Cell c = World.Cells[b];
                List<uint> neigbours = new List<uint>();
                neigbours.AddRange(c.CloseNeighbours.ToList());
                neigbours.AddRange(c.FarNeighbours.ToList());

                itterativeMotion[b] = TectonicMotion[b];

                foreach( uint i in neigbours)
                {
                    float distance = 
                        Vector3.Distance(World.Cells[b].Location, World.Cells[i].Location) - 
                        Vector3.Distance(World.Cells[b].Location + TectonicMotion[b], World.Cells[i].Location + TectonicMotion[i]);

                    float nomalized = Math.Abs(
                        Vector3.Distance(World.Cells[b].Location, World.Cells[i].Location) - 
                        Vector3.Distance(World.Cells[b].Location + TectonicMotion[b].normalized, World.Cells[i].Location + TectonicMotion[i].normalized)) / 25;

                    

                    World.Height[b] += distance;
                    itterativeMotion[b] += (TectonicMotion[i] * nomalized);
                }
                
            }

            TectonicMotion = itterativeMotion;

            World.SetTexture(TextureType.Height);
        }

        //foreach (Cell c in World.Instance.CellList)
        //{
        //    foreach (int i in c.CloseNeighbours)
        //    {
        //        float f = (Vector3.Distance(c.Location, World.Instance.CellList[i].Location) - Vector3.Distance(c.Location + c.PlateMovement, World.Instance.CellList[i].Location + World.Instance.CellList[i].PlateMovement)) * 10;
        //        c.Height += f;
        //        Debug.Log(c.ToString() + " added height " + f);
        //    }
        //}
    }

}
