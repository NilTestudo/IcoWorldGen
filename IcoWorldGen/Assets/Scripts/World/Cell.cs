using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private uint id;
    private Vector3 location;
    private List<uint> vertexes;
    private List<uint> primaryNeighbours;
    private List<uint> secondaryNeighbours;


    public Cell(uint i)
    {
        id = i;
        primaryNeighbours = new List<uint>();
        secondaryNeighbours = new List<uint>();
        vertexes = new List<uint>();
    }

    public Cell(List<uint> inputVerticies, uint i)
    {
        vertexes = new List<uint>();
        vertexes = inputVerticies;
        location = Vector3.zero;
        foreach (int v in inputVerticies)
        {
            location += World.CellPoints[v].Vector;
            World.CellPoints[v].Cells.Add(i);
        }
        location = location / 3;
        id = i;

        primaryNeighbours = new List<uint>();
        secondaryNeighbours = new List<uint>();
    }

    public Cell(uint id, Vector3 location, List<uint> vertexes, List<uint> closeNeighbours, List<uint> farNeighbours)
    {
        this.id = id;
        this.location = location;
        this.vertexes = vertexes;
        this.primaryNeighbours = closeNeighbours;
        this.secondaryNeighbours = farNeighbours;
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

    public List<uint> Vertexes
    {
        get
        {
            return vertexes;
        }

        set
        {
            vertexes = value;
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

    public List<uint> CloseNeighbours
    {
        get
        {
            return primaryNeighbours;
        }

        set
        {
            primaryNeighbours = value;
        }
    }

    public List<uint> FarNeighbours
    {
        get
        {
            return secondaryNeighbours;
        }

        set
        {
            secondaryNeighbours = value;
        }
    }

    
}