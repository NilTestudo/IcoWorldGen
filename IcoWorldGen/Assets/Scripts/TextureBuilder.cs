using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum TextureType
{
    Blank,
    Base,
    Tectonic,
    Height,
    Temperature
}


public static  class TextureBuilder
{
    private static Texture2D[] texture;

    private static int size, scale, width, height;

    public static void NewTextures()
    {
        int textureCount = Enum.GetNames(typeof(TextureType)).Length;

        size = (int)Math.Ceiling(Math.Sqrt(World.Cells.Length));
        scale = 1;
        Debug.Log("Texture size: " + size);
        width = size * scale;
        height = size * scale;
        Debug.Log("Create texture for world of " + World.Cells.Length + " cells. Size of texture aray is: " + size + "*" + size);

        texture = new Texture2D[textureCount];
        for(int i = 0; i < textureCount; i++)
        {
            texture[i] = new Texture2D(width, height, TextureFormat.ARGB32, false);
 
        }
    }

    public static Texture2D GetTexture(TextureType type)
    {
        RefreshTexture(type);
        return texture[(int)type];
    }

    //public static Texture2D CreateTexture(TextureType tType)
    //{
        
        
    //    size = (int)Math.Ceiling(Math.Sqrt(World.Cells.Length));
    //    scale = 10;
    //    Debug.Log("Texture size: " + size);
    //    int width = size*scale;
    //    int height = size*scale;
    //    Debug.Log("Create texture for world of " + World.Cells.Length + " cells. Size of texture aray is: " + size + "*" + size);

    //    switch (tType)
    //    {
    //        case TextureType.Blank:
    //            {
    //                return new Texture2D(width, height);
    //            }
    //        default:
    //            {
    //                return new Texture2D(width, height);
    //            }
    //    }
    //    Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
    //    texture.filterMode = FilterMode.Point;


    //    for (int i = 0; i < World.Cells.Length; i++)
    //    {
    //        Color col = World.CellColor[i];

    //        texture = SetCellTexture(i, col, texture);
    //    }

    //    ////Test Cor Cell 01
    //    //for (int x = 0; x <= scale - 1; x++)
    //    //{
    //    //    for (int y = 0; y <= scale - 1; y++)
    //    //    {
    //    //        texture.SetPixel(x, y, Color.red);
    //    //    }
    //    //}
        
    //    texture.name = "Generated Texture";
    //    texture.Apply();
    //    buildTexture = texture;

    //    return texture;
    //}

    public static void UpdateTexture(TextureType textureType, List<int> id)
    {
        Texture2D tempTexture = texture[(int)textureType];
        foreach (int i in id)
        {
            SetCellTexture(i, textureType);
        }
        tempTexture.Apply();
        texture[(int)textureType] = tempTexture;
    }

    private static void RefreshTexture(TextureType textureType)
    {

        texture[(int)textureType] = new Texture2D(width, height, TextureFormat.ARGB32, false);
        for(int i = 0; i < World.Cells.Length; i++)
        {
            SetCellTexture(i, textureType);
        }
        texture[(int)textureType].filterMode = FilterMode.Point;

        texture[(int)textureType].Apply();
    }

    private static void SetCellTexture(int id, TextureType textureType)
    {
        int xa, xb, ya, yb;

        if (id != 0)
        {
            //xa = (i - ((int)Math.Floor((double)(i % size)) * size));
            //xb = (i - ((int)Math.Floor((double)(i % size)) * size) + 1);

            xa = (int)(Math.Floor((double)(id / size))) * scale;
            xb = ((int)(Math.Floor((double)(id / size))) + 1) * scale - 1;
            ya = (int)(Math.Floor((double)(id % size))) * scale;
            yb = ((int)(Math.Floor((double)(id % size))) + 1) * scale - 1;

            //Debug.Log("Texture for Cell ID: " + id + " is: [" + xa + "," + ya + "] to " + "[" + xb + ", " + yb + "]");
        }
        else
        {
            xa = 0;
            xb = 1 * scale;
            ya = 0;
            yb = 1 * scale;
        }
        Color color;
        switch (textureType)
        {
            case TextureType.Base:
                color = Color.white;
                break;

            case TextureType.Tectonic:
                //Debug.Log("Drawing texture for id: " + id);
                if(World.CellTecID[id] != 0)
                {
                    color = World.Tectonics[World.CellTecID[id]-1].PlateColor;
                }
                else
                {
                    color = Color.white;
                }
                break;

            case TextureType.Height:
                float f = (World.Height[id] + 10000) / 20000;
                color = new Color(f,f,f);

                break;
                

            default:
                color = Color.white;
                break;
        }
        for (int x = xa; x <= xb; x++)
        {
            for (int y = ya; y <= yb; y++)
            {
                texture[(int)textureType].SetPixel(y, x, color);
            }
        }
    }

}