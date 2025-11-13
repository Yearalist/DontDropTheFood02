using UnityEngine;

public class Trambollın : MonoBehaviour
{
    
    public float trambolinZiplamaGucu = 15f;

    private void OnTriggerEnter(Collider other)
    {
        var controller = other.GetComponent<StarterAssets.ThirdPersonController>();
        if (controller != null)
        {
            controller.TrambolindenZipla(trambolinZiplamaGucu);
        }
    }
    
    
}
