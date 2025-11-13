using UnityEngine;

public class RagdollGroundCheck : MonoBehaviour
{
    
    
    // "Beyin" script'ine referans
    private RagdolPlayerController playerController;

    void Awake()
    {
        // Bu script, hiyerarşide yukarı doğru tırmanıp ana controller'ı bulur.
        playerController = GetComponentInParent<RagdolPlayerController>();
        
        if (playerController == null)
        {
            Debug.LogError("Bir 'RagdolPlayerController' script'i bulunamadı!");
        }
    }

    // Yere ilk dokunduğumuz an
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.isGrounded = true;
        }
    }

    // Yerde durduğumuz sürece (daha garantili yöntem)
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.isGrounded = true;
        }
    }

    // Yerden ayrıldığımız an
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Not: Bu kısım bazen zıplarken anlık olarak false'a çekip
            // tekrar true yapabilir. En güvenlisi zıplarken manuel 
            // 'false' yapmak (ana script'te yaptığımız gibi).
            
            // playerController.isGrounded = false; 
            // Şimdilik bunu kapalı bırakmak daha stabil olabilir.
        }
    }
}
