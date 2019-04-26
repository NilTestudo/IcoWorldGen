using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraControl : MonoBehaviour
{

    public float moveSpeed = 100.0F;

    public float scrollSpeed = 100.0F;

    private Vector3 cameraAnchor;

    private Quaternion cameraQuaternion;

    private GameObject target;

    private float maxCameraHeight = 10, currentCameraHeight = 5f;


    //public Text text;
    //WorldObject worldObject;

    void Start()
    {
        target = new GameObject();
        target.transform.SetPositionAndRotation(this.transform.position.normalized * World.WorldRadius, new Quaternion());
        Vector3 vec = this.transform.position.normalized * (World.WorldRadius * currentCameraHeight);
        transform.SetPositionAndRotation(vec, new Quaternion());
        transform.LookAt(Vector3.zero);
        cameraAnchor = vec;


       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        float scroll = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;

        if (horizontal != 0 || vertical != 0 || scroll != 0)
        {
            SetTargetPosition(horizontal, vertical, scroll);
        }

        this.transform.SetPositionAndRotation(target.transform.position * currentCameraHeight, Quaternion.LookRotation(-target.transform.position));

        //this.transform.LookAt(target.transform.position);
    }

    private void SetTargetPosition(float horizontal, float vertical, float scroll)
    {
        //cameraQuaternion = this.transform.rotation;

        target.transform.RotateAround(Vector3.zero, Vector3.up, horizontal * moveSpeed);
        target.transform.RotateAround(Vector3.zero, Vector3.right, vertical * moveSpeed);

        currentCameraHeight -= scroll * scrollSpeed;
        if (currentCameraHeight > maxCameraHeight)
        {
            currentCameraHeight = maxCameraHeight;
        }
        else if(currentCameraHeight < 1)
        {
            currentCameraHeight = 1;
        }

        //this.transform.Translate(-this.transform.position * scroll * scrollSpeed);


        //cameraAnchor = this.transform.position.normalized * (200 * currentCameraHeight);

        //transform.SetPositionAndRotation(cameraAnchor, cameraQuaternion);

    }



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            //Debug.Log("Mouseclick Detected");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && World.Cells != null)
            {
                Debug.Log("Hit Object" + hit.point);
                //target = hit.point;
                Cell c = World.GetCellAt(hit.point);

                //text.text = "Cell ID: " + c.Id + " \n Plate ID:  + c.PlateID +  \n Height:  + c.Height";
                //c.Color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                //TextureBuilder.UpdateSingleCellTexture((int)c.Id);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(target.transform.position, 10f);
            Gizmos.DrawWireSphere(target.transform.position*10, 10f);
            Gizmos.DrawLine(target.transform.position, target.transform.position * 10);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(target.transform.position * currentCameraHeight, 10f);

        }

    }


    //    public float speed = 0.35f;

    //    public float altitude = 4.0f;     // Height above the planet

    //    private Vector3 v3StartPos;
    //    private Vector3 v3EndPos;

    //    private float timer = 1.1f;



    //    void Update()
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hit;
    //            if (Physics.Raycast(ray, out hit) && World.Cells != null)
    //            {
    //                Debug.Log("Hit Object" + hit.point);
    //                target = hit.point;
    //                Cell c = World.GetCellAt(hit.point);
    //                //text.text = "Cell ID: " + c.Id + " \n Plate ID:  + c.PlateID +  \n Height:  + c.Height";
    //                //c.Color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    //                //TextureBuilder.UpdateSingleCellTexture((int)c.Id);

    //                timer = 0.0f;
    //                v3StartPos = transform.position - target;
    //                v3EndPos = (hit.point - target).normalized * altitude;

    //            }

    //        }

    //        if (timer <= 1.0)
    //            transform.position = target + Vector3.Slerp(v3StartPos, v3EndPos, timer);

    //        timer += Time.deltaTime * speed;
    //    }
}


