
namespace Mirror
{
    public abstract class GameObjectSpawner : NetworkBehaviour
    {
        protected virtual void InstanciatePlayer()
        {
        }

        protected virtual void InstanciateAssets()
        {
        }

        protected virtual void GetPlayerGameObject()
        {
        }

        protected virtual void GetNetworkedPlayerControls()
        {
        }

        protected virtual void SetAssetGameObject()
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

        protected virtual void InitializeSpawnedAssets()
        {
        }
    }
}