// Source : https://unity.com/how-to/create-modular-and-maintainable-code-observer-pattern
using Mirror;
using System;
using UnityEngine;

public class RoleSelection : NetworkBehaviour
{
    public event Action OnPlayerRoleSelected;

    public void OnSelectRunner()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Debug.Log("Runner slected!");
        GameObject go = Instantiate(NetworkManager.singleton.spawnPrefabs[0], transform);
        OnPlayerRoleSelected?.Invoke();
        gameObject.SetActive(false);
    }

    public void OnSelectHuntner()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Debug.Log("Hunter slected!");
        GameObject go = Instantiate(NetworkManager.singleton.spawnPrefabs[1], transform);
        OnPlayerRoleSelected?.Invoke();
        gameObject.SetActive(false);
    }
}