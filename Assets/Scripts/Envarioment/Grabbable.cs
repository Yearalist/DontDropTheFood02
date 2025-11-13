using UnityEngine;
using System.Collections.Generic;

public class Grabbable : MonoBehaviour
{
    public Transform[] grabPoints; // Inspector'dan tutma noktaları ekleyin
    private Dictionary<Transform, bool> grabPointStatus = new Dictionary<Transform, bool>();

    void Start()
    {
        foreach (Transform point in grabPoints)
        {
            grabPointStatus[point] = false; // Başlangıçta tüm noktalar boş
        }
    }

    // **En yakın ve boş olan noktayı bul**
    public Transform GetClosestAvailableGrabPoint(Vector3 position)
    {
        Transform closestPoint = null;
        float closestDistance = float.MaxValue;

        foreach (Transform point in grabPoints)
        {
            if (!grabPointStatus[point]) // Eğer nokta boşsa
            {
                float distance = Vector3.Distance(position, point.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = point;
                }
            }
        }

        return closestPoint;
    }

    // **Bir noktayı rezerve et**
    public void ReserveGrabPoint(Transform point)
    {
        if (point != null && grabPointStatus.ContainsKey(point))
        {
            grabPointStatus[point] = true;
        }
    }

    // **Bir noktayı boşalt**
    public void ReleaseGrabPoint(Transform point)
    {
        if (point != null && grabPointStatus.ContainsKey(point))
        {
            grabPointStatus[point] = false;
        }
    }
    
}
