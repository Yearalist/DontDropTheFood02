using UnityEngine;

using Unity.Cinemachine;
using Unity.Netcode;

public class CameraManager : NetworkBehaviour
{
    public CinemachineCamera cinemachineCamera;

    private void Start()
    {
       //cinemachineCamera.gameObject.SetActive(false); // Başta kapalı
    }

    public void SwitchToPlayerCamera(Transform followTarget)
    {
        var cinemachineCamera = FindObjectOfType<CinemachineCamera>();
        if (cinemachineCamera == null)
        {
            Debug.LogError("VCam not found!");
            return;
        }

        cinemachineCamera.Follow = followTarget;
        cinemachineCamera.LookAt = followTarget;

        Debug.Log("Camera now following: " + followTarget.name);
    }

}
