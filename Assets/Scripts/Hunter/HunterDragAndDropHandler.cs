using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runhunt.Hunter
{
    public class HunterDragAndDropHandler : NetworkBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public GameObject cubePrefab;
        public Camera TopDownCam;
        public LayerMask raycastLayer;
        private Canvas canvas;
        private GameObject instantiatedCube;

        void Start()
        {
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //if (!isLocalPlayer)
            //    return; // Only execute on the local player

            Ray ray = TopDownCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
            {
                // Instantiate the cube only on the server
                if (isServer)
                {
                    instantiatedCube = Instantiate(cubePrefab, hit.point, Quaternion.identity);
                    NetworkServer.Spawn(instantiatedCube);
                }
                else
                {
                    CmdSpawnCube(hit.point); // Request the server to spawn the cube
                }
            }
        }

        [Command] // This method is called on the servercastlenau
        void CmdSpawnCube(Vector3 position)
        {
            instantiatedCube = Instantiate(cubePrefab, position, Quaternion.identity);
            NetworkServer.Spawn(instantiatedCube);
        }


        public void OnDrag(PointerEventData eventData)
        {

            if (instantiatedCube == null)
                return;

            Ray ray = TopDownCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
            {
                Debug.Log("OnDrag pos: " + hit.point);
                instantiatedCube.transform.position = hit.point;
                CmdUpdatePosition(hit.point);
            }
        }

        [Command]
        void CmdUpdatePosition(Vector3 newPosition)
        {
            // Update the position on the server
            instantiatedCube.transform.position = newPosition;
            RpcUpdatePosition(newPosition);
        }

        [ClientRpc]
        void RpcUpdatePosition(Vector3 newPosition)
        {
            // Update the position on all clients
            instantiatedCube.transform.position = newPosition;
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");

            if (!isLocalPlayer || instantiatedCube == null)
                return;

            //Ray ray = TopDownCam.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;

            //if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer))
            //{
            //    instantiatedCube.transform.position = hit.point;

            //}
        }

    }
}