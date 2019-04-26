using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldObject : MonoBehaviour
{
    public GameObject map;
    private MeshRenderer renderer;
    private MeshFilter mesh;

    // Use this for initialization
    void Start()
    {
        mesh = this.GetComponent<MeshFilter>();
        renderer = this.GetComponent<MeshRenderer>();


        World.Texture += UpdateTexture;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        World.Texture -= UpdateTexture;
    }

    public void UpdateMesh()
    {
        Debug.Log("Update Mesh");
        mesh.mesh = MeshCreator.WorldMesh();

    }

    

    public void UpdateTexture(TextureType type)
    {
        Texture2D tex = TextureBuilder.GetTexture(type);

        renderer.sharedMaterial.mainTexture = tex;
        Debug.Log("Changed Texture");
    }

    

}
