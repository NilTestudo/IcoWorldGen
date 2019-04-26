using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class World
{
    public delegate void ChangeTexture(TextureType type);
    public static event ChangeTexture Texture;

    //Basic World Data
    private static Cell[] cells;
    private static CellPoint[] cellPoints;

    private static int worldRadius = 6371;
    private static float worldCellScale;


    //Secondary Data
    private static float[] height;
    private static Color[] cellColor;

    //Tectonic Data
    private static TectonicPlate[]tectonics;
    private static UInt16[] cellTecID;

    public static Cell[] Cells
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

    public static CellPoint[] CellPoints
    {
        get
        {
            return cellPoints;
        }

        set
        {
            cellPoints = value;
        }
    }

    public static TectonicPlate[] Tectonics
    {
        get
        {
            return tectonics;
        }

        set
        {
            tectonics = value;
        }
    }

   public static UInt16[] CellTecID
    {
        get
        {
            return cellTecID;
        }

        set
        {
            cellTecID = value;
        }
    }
    public static Color[] CellColor
    {
        get
        {
            return cellColor;
        }

        set
        {
            cellColor = value;
        }
    }

    public static float[] Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }

    public static float WorldCellScale
    {
        get
        {
            return worldCellScale;
        }

        set
        {
            worldCellScale = value;
        }
    }

    public static int WorldRadius
    {
        get
        {
            return worldRadius;
        }

        set
        {
            worldRadius = value;
        }
    }

    public static void SetTexture(TextureType type)
    {
        if (Texture != null)
        {
            Texture(type);
        }
    }


    public static Cell GetCellAt(Vector3 inVector)
    {
        List<Cell> cellList = new List<Cell>();
        cellList.AddRange(cells);
        List<Cell> nearestCells = cellList.FindAll( item => Vector3.Distance(item.Location, inVector.normalized) < Vector3.Distance(cellPoints[0].Vector, cellPoints[1].Vector));
        Cell closest = null;
        foreach (Cell c in nearestCells)
        {
            if (closest == null)
            {
                closest = c;
            }

            if (Vector3.Distance(c.Location, inVector.normalized) < Vector3.Distance(closest.Location, inVector.normalized))
            {
                closest = c;
            }

        }

        return closest;
    }



}
