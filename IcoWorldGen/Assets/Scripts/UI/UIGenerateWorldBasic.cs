using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIGenerateWorldBasic : MonoBehaviour {

    int divisions;
    int radius;

    WorldObject worldObject;
    InputField inputField;
    Dropdown dropdown;
    Button nxtButton;
    public Text descriptText;


    private void Awake()
    {
        worldObject = GameObject.FindGameObjectWithTag("World").GetComponent<WorldObject>();
        inputField = this.transform.GetComponentInChildren<InputField>();
        dropdown = this.transform.GetComponentInChildren<Dropdown>();

    }


    // Use this for initialization
    void Start () {
        divisions = dropdown.value;
        Int32.TryParse(inputField.text, out radius);
        SetEdditetText();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnEditetDivisionInput()
    {
        int a = dropdown.value;
        if (a < 0)
        {
            a = 0;
        }
        else if (a > 5)
        {
            a = 5;
        }
        divisions = a;
        Debug.Log("Divisions level at: " + divisions);
        SetEdditetText();
    }

    public void OnEditedRadiusInput()
    {
        int a = 1;

        Int32.TryParse(inputField.text, out a);

        radius = a;
        Debug.Log("Radius set to: " + divisions + " Km");
        SetEdditetText();
    }
 

    public void Generate()
    {
        Debug.Log("Generate basic world");
        World.WorldRadius = radius;
        GenerateBasicWorld.Generate(divisions);

        worldObject.UpdateMesh();
        World.SetTexture(TextureType.Base);
        
    }

    void SetEdditetText()
    {
        descriptText.text = "Subdivisions Set to: "+ divisions + ". \n";
        int cellnr = 20;
        for(int i = 1; i <= divisions; i++)
        {
            cellnr = cellnr * 4;
        }
        descriptText.text += "Number of world cells: " + cellnr + ". \n";

        descriptText.text += "World Radius = " + radius + "Km. \n";
        descriptText.text += "World Diameter = " + radius * 2 + "Km. \n";
        double surfaceArea = (4 * Math.PI * Math.Pow(radius, 2));
        descriptText.text += "World Surface Area = " + (int)surfaceArea + "Km^2. \n";
        descriptText.text += "Surface Area per Cell = " + (int)(surfaceArea/cellnr) + "Km^2. \n";


    }
}
