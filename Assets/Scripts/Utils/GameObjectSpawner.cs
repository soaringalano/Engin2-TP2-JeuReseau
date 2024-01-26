using Mirror;

public abstract class GameObjectSpawner : NetworkBehaviour
{
    protected virtual void InstanciateAssets()
    {
    }

    protected virtual void GetPlayerGameObject()
    {
    }

    protected virtual void GetNetworkedPlayerControls()
    {
    }

    protected virtual void SetCameraInNetworkedPlayerControls()
    {
    }

    protected virtual void SetTheCameraFollow()
    {
    }

    protected virtual void SetTheCameraLookAt()
    {
    }
}
