using UnityEngine;

public class TepsiController : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ağırlık merkezini ayarla
        rb.centerOfMass = Vector3.zero; // Veya istediğin özel bir offset
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Daha smooth hareket için
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Fizik hatalarını azaltır

        rb.linearDamping = 2f;          // Hava direnci
        rb.angularDamping = 5f;   // Dönüş direnci
    }
}
