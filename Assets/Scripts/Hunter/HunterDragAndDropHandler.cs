using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HunterDragAndDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform dragTransform; 
    public GameObject cubePrefab;
    public Camera TopDownCam;

    public LayerMask raycastLayer;

    private Canvas canvas;
    private RectTransform canvasRect;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    void Update()
    {
       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Set the parent of the dragging object to the canvas so it stays on top
        Debug.Log("OnBeginDrag");
        dragTransform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
       
        // Follow the cursor position while dragging
       RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        //dragTransform.position = canvas.transform.TransformPoint(localPoint);
        cubePrefab.transform.position = canvas.transform.TransformPoint(localPoint);

        Vector3 mousePosition = TopDownCam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Set the z-coordinate to 0 to match the game world
        Debug.Log("OnDrag Mousepos x: " + mousePosition.x + " y: " + mousePosition.y);
        // Update the position of the cube prefab to follow the mouse position
        cubePrefab.transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        // Release the object from the canvas and instantiate a cube prefab at the release position
        dragTransform.SetParent(canvasRect, true);

        // Convert the mouse position to world space
        //Vector3 mousePosition = TopDownCam.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition.z = 0f; // Set the z-coordinate to 0 to match the game world

        // Instantiate the cube prefab at the mouse position
        //Instantiate(cubePrefab, mousePosition, Quaternion.identity);
        //cubePrefab.transform.position = mousePosition;

        Ray ray = TopDownCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
        {
            Debug.Log("hite x: " + hit.point.x + " z: " + hit.point.z);
            //Debug.Log("RaycastFuck");
            // lace the cube on top of the plane
            Vector3 cubePosition = new Vector3(hit.point.x, -610, hit.point.z);
            cubePrefab.transform.position = cubePosition;
            Instantiate(cubePrefab, cubePosition, Quaternion.identity);
        }


    }

}
