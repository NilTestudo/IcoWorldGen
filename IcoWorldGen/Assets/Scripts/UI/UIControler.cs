using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIControler : MonoBehaviour
{
    public GameObject[] MenuItems;

    private Vector3 startLocation;

    private int open;

    public void Awake()
    {
        startLocation = MenuItems[0].transform.position;
    }


    public void OnEnable()
    {        
        OpenPanel(0);
    }

    //Closes the currently open panel and opens the provided one.
    //It also takes care of handling the navigation, setting the new Selected element.
    public void OpenPanel(int a)
    {

        if (open == a)
            return;

        //Activate the new Screen hierarchy so we can animate it.
        MenuItems[a].gameObject.SetActive(true);
        
        //Move the Screen to front.
        MenuItems[a].transform.SetAsLastSibling();

        DisablePanel(MenuItems[open]);
        OpenPanel(MenuItems[a]);

        open = a;
        

        //Set an element in the new screen as the new Selected one.
        GameObject go = FindFirstEnabledSelectable(MenuItems[a]);
        SetSelected(go);
    }

    public void NextUI()
    {
        if (open < MenuItems.Length)
        {
            OpenPanel(open + 1);
        }
    }

    public void PreviusUI()
    {
        if (open != 0)
        {
            OpenPanel(open - 1);
        }
       
    }

    //Finds the first Selectable element in the providade hierarchy.
    static GameObject FindFirstEnabledSelectable(GameObject gameObject)
    {
        GameObject go = null;
        var selectables = gameObject.GetComponentsInChildren<Selectable>(true);
        foreach (var selectable in selectables)
        {
            if (selectable.IsActive() && selectable.IsInteractable())
            {
                go = selectable.gameObject;
                break;
            }
        }
        return go;
    }

    

    //Coroutine that will detect when the Closing animation is finished and it will deactivate the
    //hierarchy.
    IEnumerator DisablePanel(GameObject go)
    {
        
        go.SetActive(false);
        return null;
    }

    IEnumerator OpenPanel(GameObject go)
    {
        go.SetActive(true);

        //Move the Screen to front.
        go.transform.SetAsLastSibling();



        return null;
    }

    //Make the provided GameObject selected
    //When using the mouse/touch we actually want to set it as the previously selected and 
    //set nothing as selected for now.
    private void SetSelected(GameObject go)
    {
        //Select the GameObject.
        EventSystem.current.SetSelectedGameObject(go);

        //If we are using the keyboard right now, that's all we need to do.
        var standaloneInputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
        if (standaloneInputModule != null)
            return;

        //Since we are using a pointer device, we don't want anything selected. 
        //But if the user switches to the keyboard, we want to start the navigation from the provided game object.
        //So here we set the current Selected to null, so the provided gameObject becomes the Last Selected in the EventSystem.
        EventSystem.current.SetSelectedGameObject(null);
    }
}