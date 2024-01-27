using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RotateTerrain2 : MonoBehaviour
{
    [SerializeField]  GameObject m_terrainPlane;
    [SerializeField]  private float rotationSpeed;

    [SerializeField] private float maxRotationAngle = 15f;

    private bool isRotating = false;
    private Vector3 previousMousePosition;


    private float currentRotationX = 0f;
    private float currentRotationZ = 0f;

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            previousMousePosition = Input.mousePosition;
        }

        // Check for right mouse button up
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            Vector3 mouseDelta = Input.mousePosition - previousMousePosition;

            float angleZ = mouseDelta.x * rotationSpeed;
            float angleX = mouseDelta.y * rotationSpeed;

            currentRotationX += angleX;
            currentRotationZ += angleZ;


            currentRotationX = Mathf.Clamp(currentRotationX, -maxRotationAngle, maxRotationAngle);
            currentRotationZ = Mathf.Clamp(currentRotationZ, -maxRotationAngle, maxRotationAngle);

            // Calculate delta rotation to apply
            float deltaRotationX = currentRotationX - m_terrainPlane.transform.rotation.eulerAngles.x;
            float deltaRotationZ = currentRotationZ - m_terrainPlane.transform.rotation.eulerAngles.z;

            // Apply rotation
            m_terrainPlane.transform.Rotate(Vector3.right, deltaRotationX, Space.World);
            m_terrainPlane.transform.Rotate(Vector3.forward, deltaRotationZ, Space.World);
        }

    }
}
