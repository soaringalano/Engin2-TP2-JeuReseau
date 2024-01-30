using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnReadyStateChanged))]
    private bool isReady = false;


    public override void OnStartClient()
    {
        base.OnStartClient();

        // For example, load scenes, allow player to choose an avatar, etc.
        // ...

        // Once all pre-game work is done and assets are loaded, call Ready to enter the "ready" state
        if (isLocalPlayer)
        {
            CmdSetReadyState(true);
        }
    }

    // Method to set the ready state on the server
    [Command]
    private void CmdSetReadyState(bool readyState)
    {
        isReady = readyState;

        // Notify all clients about the ready state change
        RpcUpdateReadyState(isReady);
    }

    // Method to update the ready state on all clients
    [ClientRpc]
    private void RpcUpdateReadyState(bool readyState)
    {
        // Update the ready state on the client side
        isReady = readyState;

        // Perform any client-side actions based on the ready state
        // ...

        // If the client is ready, start receiving updates from the server
        if (isReady)
        {
            // Perform actions when entering the "ready" state
            // ...
        }
    }

    // Hook method to handle changes in the ready state on the client side
    private void OnReadyStateChanged(bool oldState, bool newState)
    {
        // Perform any actions when the ready state changes on the client side
        // ...
    }
}
