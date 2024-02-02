// Source : https://unity.com/how-to/create-modular-and-maintainable-code-observer-pattern
using Mirror;
using System;
using Unity.VisualScripting;
using UnityEngine;
using static Mirror.NetworkManager;

public class RoleSelection : NetworkBehaviour
{
    public void OnSelectRunner()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Debug.Log("Runner slected!");
        //GameObject go = Instantiate(NetworkManager.singleton.spawnPrefabs[0], transform);
        singleton.RunHuntPlayer.SetRole(Role.Runner);
        //GameEventManager.GetInstance().RaiseOnPlayerRoleSelectedEvent();
        gameObject.SetActive(false);
    }

    public void OnSelectHuntner()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Debug.Log("Hunter slected!");
        //GameObject go = Instantiate(NetworkManager.singleton.spawnPrefabs[1], transform);
        singleton.RunHuntPlayer.SetRole(Role.Runner);
        //GameEventManager.GetInstance().RaiseOnPlayerRoleSelectedEvent();
        gameObject.SetActive(false);
    }
}