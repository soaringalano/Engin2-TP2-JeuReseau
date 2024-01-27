using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RotateTerrain : NetworkBehaviour
{
    [SerializeField]
    private Terrain m_terrain;
    [SerializeField]
    GameObject m_terrainPlane;
    [SerializeField]
    private float m_terrainRotateSpeed;


    //public void SpawnPrefab()
    //{
    //    // Check if we are the server
    //    if (isServer)
    //    {
    //        // Instantiate the prefab using the NetworkManager
    //        GameObject prefabInstance = Instantiate(m_terrainPlane);

    //        // Spawn the instantiated prefab across the network
    //        NetworkServer.Spawn(prefabInstance);
    //    }
    //}

    public override void OnStartServer()
    {
        base.OnStartServer();

        // Spawn the prefab only on the server
        GameObject prefabInstance = Instantiate(m_terrainPlane);

        // Spawn the instantiated prefab across the network
        NetworkServer.Spawn(prefabInstance);
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
           
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Debug.Log("right button hold mx" + mouseX + " mx " + mouseY );

            //m_terrainPlane.transform.RotateAround(Vector3.up, -mouseX);
            //m_terrainPlane.transform.rotation *= Quaternion.Euler(mouseY, -mouseX, 0);

            m_terrainPlane.transform.Rotate(Vector3.up, 0f, Space.World); // No rotation around Y axis
            m_terrainPlane.transform.Rotate(Vector3.right, mouseY * m_terrainRotateSpeed, Space.World);
            m_terrainPlane.transform.Rotate(Vector3.forward, -mouseX * m_terrainRotateSpeed, Space.World);
            //m_terrain.transform.Rotate(Vector3.up, 0f, Space.World); // No rotation around Y axis
            //m_terrain.transform.Rotate(Vector3.right, mouseY * m_terrainRotateSpeed, Space.World);
            //m_terrain.transform.Rotate(Vector3.forward, -mouseX * m_terrainRotateSpeed, Space.World);
        }
       
    }
}
