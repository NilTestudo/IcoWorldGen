using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public GameObject messagePrefab;

    private GameObject msg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Options()
    {
        DestroyMessage();
        msg = Instantiate(messagePrefab);
        Text[] txt = msg.GetComponentsInChildren<Text>();
        txt[0].text = "Options Not available";
        txt[1].text = "OK";
        Button btn = msg.GetComponentInChildren<Button>();
        btn.onClick.AddListener(DestroyMessage);
    }

    public void DestroyMessage()
    {
        if(msg != null)
        {
            Destroy(msg);
        }
    }



}
