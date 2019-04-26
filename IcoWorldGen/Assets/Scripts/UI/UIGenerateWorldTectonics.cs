using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[ExecuteInEditMode]
public class UIGenerateWorldTectonics : MonoBehaviour
{
    private UIControler controler;

    private int continents;
    private float oceanPercentage = 0.5f, linear = 1.5f, angular = 1.5f;

    private InputField[] input;

    private Slider slider;
    public Text opText;

        // Use this for initialization
        void Start()
    {
        input = GetComponentsInChildren<InputField>();

        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(OnSliderChange);
        OnSliderChange(oceanPercentage);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSliderChange(float f)
    {
        oceanPercentage = f;
        opText.text = "Ocean: " + (int)(oceanPercentage * 100) + "%";
    }

    public void Generate()
    {
        Int32.TryParse(input[0].text, out continents);

        float.TryParse(input[1].text, out linear);
        float.TryParse(input[2].text, out angular);

        GenerateTectonics.GenerateContinents(continents, oceanPercentage, linear, angular);
        World.SetTexture(TextureType.Tectonic);
        StartCoroutine("Grow");
    }

    IEnumerator Grow()
    {
        bool growing = false;
        Debug.Log("Started Coroutine");
        while(!growing)
        {
            growing = GenerateTectonics.GrowContinents();
            World.SetTexture(TextureType.Tectonic);

            yield return null;
        }
        World.SetTexture(TextureType.Tectonic);

        Debug.Log("Ended Coroutine");

    }
}
