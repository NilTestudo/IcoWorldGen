using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TectonicPlate
{
    private UInt16 id;
    private Color plateColor;
    private bool isOceanPlate;
    private float height;
    private Vector3 location;
    private Vector3 movement;
    private Matrix4x4 localMatrix;
    private List<uint> plateCellList;
    private List<uint> checkList;
    private float angular;

    public Color PlateColor
    {
        get
        {
            return plateColor;
        }

        set
        {
            plateColor = value;
        }
    }

    public Matrix4x4 LocalMatrix
    {
        get
        {
            return localMatrix;
        }

        set
        {
            localMatrix = value;
        }
    }

    public Vector3 Location
    {
        get
        {
            return location;
        }

        set
        {
            location = value;
        }
    }

    public Vector3 Movement
    {
        get
        {
            return movement;
        }

        set
        {
            movement = value;
        }
    }

    public bool IsOceanPlate
    {
        get
        {
            return isOceanPlate;
        }

        set
        {
            isOceanPlate = value;
        }
    }

    public List<uint> PlateCellList
    {
        get
        {
            return plateCellList;
        }

        set
        {
            plateCellList = value;
        }
    }

    public TectonicPlate(UInt16 id, Color color, uint origin, float height, bool ocean, float movement, float rotation)
    {
        this.id = (UInt16)(id+1);
        this.plateColor = color;
        this.height = height;
        plateCellList = new List<uint>();
        plateCellList.Add(origin);
        World.CellTecID[origin] = this.id;
        World.Height[origin] = this.height;
        this.location = World.Cells[origin].Location;
        this.localMatrix = Matrix4x4.TRS(this.Location, Quaternion.LookRotation(this.Location), Vector3.one);
        //Debug.Log("Plate " + this.id + " createed with matrix: " + localMatrix.ToString());
        checkList = new List<uint>();
        this.angular = UnityEngine.Random.Range(-rotation, rotation);

        this.movement = LocalMatrix.MultiplyPoint3x4(new Vector3(UnityEngine.Random.Range(-movement, movement), UnityEngine.Random.Range(-movement, movement), 0)) - this.Location;
        //origin.PlateMovement = this.movement;
        this.isOceanPlate = ocean;

        //this.movement = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f),UnityEngine.Random.Range(-0.05f, 0.05f),0);

        foreach (uint i in World.Cells[origin].CloseNeighbours)
        {
            if (World.CellTecID[i] == 0)
            {
                checkList.Add(i);
            }
        }

        //Debug.Log("Plate " + id + " created with " + checkList.Count + " in checklist");

    }


    public bool Grow()
    {

        if (checkList.Count != 0)
        {
            int chance = UnityEngine.Random.Range(0, 100);
            if (chance > 50)
            {
                List<uint> temp = new List<uint>();

                int count1 = 0;
                Quaternion rotation = Quaternion.Euler(0, 0, angular * 50);

                foreach (uint c in checkList)
                {
                    if (World.CellTecID[(int)c] == 0)
                    {
                        Cell cell = World.Cells[(int)c];
                        plateCellList.Add(c);
                        World.CellTecID[(int)c] = this.id;
                        World.Height[(int)c] = this.height;

                        GenerateTectonics.TectonicMotion[cell.Id] = movement;

                        // Input rotation
                        Vector3 vec = rotation * LocalMatrix.inverse.MultiplyPoint3x4(cell.Location);
                        GenerateTectonics.TectonicMotion[cell.Id] += LocalMatrix.MultiplyPoint3x4(vec) - cell.Location;

                        count1++;

                        foreach (int i in cell.CloseNeighbours)
                        {
                            if (World.CellTecID[i] == 0)
                            {
                                temp.Add((uint)i);

                            }
                        }

                    }
                }
                //Debug.Log("Tectonic Plate " + id + " grown by " + count1);
                checkList = temp;
                //Debug.Log(temp.Count + " added to " + id + " checklist");
            }
        }
        else
        {
            return true;
        }


        return false;

    }


}
