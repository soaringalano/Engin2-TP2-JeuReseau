// Source : https://unity.com/how-to/create-modular-and-maintainable-code-observer-pattern
using Mirror;
using UnityEngine;

public class GameEventManager : NetworkBehaviour, GameEventObserver
{
    [field: SerializeField]
    private GameObject Scene {  get; set; }
    [field: SerializeField]
    private GameEventSubject gameEventSubject;

    private void Start()
    {
        gameEventSubject.AddObserver(this);
    }

    void GameEventObserver.OnPlayerRoleSelected()
    {
        Scene.SetActive(true);
    }
}