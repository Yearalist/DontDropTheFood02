using UnityEngine;
using Unity.Netcode;
public class PlayerSetup : NetworkBehaviour
{
    public Transform followTarget;

    private void Start()
    {
        if (!IsOwner) return;

        var cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
        {
            cameraManager.SwitchToPlayerCamera(followTarget);
        }
    }
}
