using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CellPoint
{
    private uint id;
    private Vector3 vector;
    private List<uint> cells;

    public CellPoint(Vector3 vec)
    {
        cells = new List<uint>();
        vector = vec;
    }

    public CellPoint(uint id, Vector3 vector, List<uint> cells)
    {
        this.id = id;
        this.vector = vector;
        this.cells = cells;
    }


    public Vector3 Vector
    {
        get
        {
            return vector;
        }

        set
        {
            vector = value;
        }
    }

    public List<uint> Cells
    {
        get
        {
            return cells;
        }

        set
        {
            cells = value;
        }
    }

    public uint Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }
}
