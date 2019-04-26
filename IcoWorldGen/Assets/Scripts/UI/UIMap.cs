using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;

public class UIMap : MonoBehaviour
{
    private Dropdown dropdown;
    private Image image;


    private bool displayInertia = false;
    public Mesh mesh;
    public Material material;

    // Use this for initialization
    void Start()
    {
        dropdown = GetComponentInChildren<Dropdown>();
        dropdown.options.Clear();
        List<string> ttypes = Enum.GetNames(typeof(TextureType)).ToList<string>();

        dropdown.AddOptions(ttypes);
        dropdown.RefreshShownValue();

        image = this.GetComponentsInChildren<Image>()[3];

        dropdown.onValueChanged.AddListener(ChangeTexture);
        World.Texture += OnChangedTexture;

    }

    // Update is called once per frame
    void Update()
    {
        if (displayInertia)
        {
            foreach (TectonicPlate tec in World.Tectonics)
            {
                //Graphics.DrawMesh(mesh, tec.Location*100, Quaternion.identity, material, 0);
                material.SetColor(0, tec.PlateColor);
                Graphics.DrawMesh(mesh, tec.Location * 100, tec.LocalMatrix.rotation, material, 0);

                int wRadius = World.WorldRadius;
                float cellScale = World.WorldCellScale;

                foreach(int i in tec.PlateCellList)
                {
                    if (GenerateTectonics.TectonicMotion[i].magnitude != 0)
                    {
                        Matrix4x4 matrix = Matrix4x4.TRS(World.Cells[i].Location * wRadius, Quaternion.LookRotation(GenerateTectonics.TectonicMotion[i]), new Vector3(GenerateTectonics.TectonicMotion[i].magnitude * cellScale, GenerateTectonics.TectonicMotion[i].magnitude * cellScale, GenerateTectonics.TectonicMotion[i].magnitude * cellScale));
                        Graphics.DrawMesh(mesh, matrix, material, 0);
                    }
                }
                //Debug.Log("Mesh Created for " + tec.Location);
            }
            //foreach (Cell c in World.Cells)
            //{
            //    Matrix4x4 matrix = Matrix4x4.TRS(c.Location * 105, Quaternion.LookRotation(GenerateTectonics.ItterativeMotion[c.Id]), new Vector3(GenerateTectonics.ItterativeMotion[c.Id].magnitude, GenerateTectonics.ItterativeMotion[c.Id].magnitude, GenerateTectonics.ItterativeMotion[c.Id].magnitude));
            //    Graphics.DrawMesh(mesh, matrix, material, 0);
            //    //Graphics.DrawMesh(mesh, c.Location * 100, Quaternion.LookRotation(GenerateTectonics.ItterativeMotion[c.Id]),material, 0);
            //}
        }
    }

    private void OnDestroy()
    {
        World.Texture -= OnChangedTexture;

    }

    void ChangeTexture(int i)
    {
        TextureType t = (TextureType)i;
        World.SetTexture(t);
    }

    void OnChangedTexture(TextureType type)
    {


        if (type == TextureType.Tectonic || type == TextureType.Height)
        {
            displayInertia = true;
        }
        else
        {
            displayInertia = false;

        }

        Texture2D tex = TextureBuilder.GetTexture(type);
        Sprite spr = Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), Vector2.zero);

        image.sprite = spr;

        dropdown.value = ((int)type);
        Debug.Log("UI Recived ChangeTexture Signal");
    }

    void OnDrawGizmos()
    {

        if (displayInertia)
        {
            int wRadius = World.WorldRadius;
            float cellScale = World.WorldCellScale;

            Gizmos.color = Color.blue;
            foreach (TectonicPlate tec in World.Tectonics)
            {

                Gizmos.DrawSphere(tec.Location * wRadius, 1f);
                Gizmos.DrawLine(tec.Location * wRadius, tec.Location * wRadius * 1.1f);
                //Debug.Log("Mesh Created for " + tec.Location);
            }
            Gizmos.color = Color.yellow;
            foreach (Cell c in World.Cells)
            {
                Gizmos.DrawLine(c.Location * wRadius, (c.Location * wRadius) + GenerateTectonics.TectonicMotion[c.Id]*cellScale);
            }
        }

    }

}
