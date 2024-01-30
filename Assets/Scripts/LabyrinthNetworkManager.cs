// source : https://mirror-networking.gitbook.io/docs/manual/guides/gameobjects/custom-character-spawning

using Mirror;
using System.Linq;
using UnityEngine;

public class LabyrinthNetworkManager : NetworkManager
{
    public struct CreateCharacterMessage : NetworkMessage
    {
        public Role role;
        public string name;
    }

    public enum Role
    {
        Runner,
        Hunter,
        Count
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        // you can send the message here, or wherever else you want
        CreateCharacterMessage characterMessage = new CreateCharacterMessage
        {
            role = Role.Runner,
            name = "Joe Gaba Gaba",
        };

        NetworkClient.Send(characterMessage);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, CreateCharacterMessage message)
    {
        GameObject playerPrefab = null;
        if (selectedPrefabIndex < playerPrefabs.Count())
        {
            playerPrefab = playerPrefabs[selectedPrefabIndex];
        }

        // playerPrefab is the one assigned in the inspector in Network
        // Manager but you can use different prefabs per race for example
        GameObject gameobject = Instantiate(playerPrefab);

        // Apply data from the message however appropriate for your game
        // Typically Player would be a component you write with syncvars or properties
        Player player = gameobject.GetComponent<Player>();
        if (player == null) Debug.LogError("Player component not found on the instantiated gameobject.");

        player.m_role = message.role.ToString();

        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }
}
