using UnityEngine;

public class BalancePlatform : MonoBehaviour
{
    public Transform player; // Oyuncunun transformu
    public float tiltAmount = 10f; // Ne kadar eğilsin
    public float smoothSpeed = 2f; // Yumuşaklık

    private Quaternion defaultRotation;

    void Start()
    {
        defaultRotation = transform.rotation;
    }

    void Update()
    {
        Vector3 localPlayerPos = transform.InverseTransformPoint(player.position);
        float targetZ = Mathf.Clamp(localPlayerPos.x * tiltAmount, -tiltAmount, tiltAmount);
        Quaternion targetRotation = Quaternion.Euler(0, 0, -targetZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}
