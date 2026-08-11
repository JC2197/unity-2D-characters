using UnityEngine;

#if FISHNET
using FishNet.Object;
#endif

namespace JoeConticello.Characters2D.FishNet
{
#if FISHNET
    // This script will be a NetworkBehaviour so that we can use the 
    // OnStartClient override.
    public class FishnetPlayerCamera : NetworkBehaviour
    {
        [SerializeField] private Camera _cameraPrefab;
        [SerializeField] private Transform _cameraHolder;

        // This method will run on the client once this object is spawned.
        public override void OnStartClient()
        {
            // Since this will run on all clients that this object spawns for
            // we need to only instantiate the camera for the object we own.
            if (IsOwner)
                Instantiate(_cameraPrefab, _cameraHolder.position, _cameraHolder.rotation, _cameraHolder);
        }
    }
#else
    public sealed class FishnetPlayerCamera : MonoBehaviour
    {
    }
#endif
}