using UnityEngine;
using System.Collections.Generic;
public class CharacterGrabber : MonoBehaviour
{
    
    
    
    
    public Transform leftHand;
    public Transform rightHand;
    public KeyCode grabKey;
    public float grabRadius = 2f;

    private Grabbable grabbedObject;
    private Transform grabbedPointLeft;
    private Transform grabbedPointRight;

    void Update()
    {
        if (Input.GetKeyDown(grabKey))
        {
            if (grabbedObject == null)
            {
                TryGrab();
            }
            else
            {
                Release();
            }
        }
    }

    
    void AttachHand(Transform hand, Transform point, Vector3 offset)
    {
        hand.position = point.position + offset;
        hand.rotation = point.rotation;
        FixedJoint joint = hand.gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = grabbedObject.GetComponent<Rigidbody>();
    }
    
    void TryGrab()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, grabRadius);
        foreach (Collider col in colliders)
        {
            Grabbable grabbable = col.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                Transform point = grabbable.GetClosestAvailableGrabPoint(transform.position);
                if (point != null)
                {
                    grabbedObject = grabbable;
                    grabbedPointLeft = point;
                    grabbedObject.ReserveGrabPoint(grabbedPointLeft);

                    // Sol el direkt noktanın üstünde
                    AttachHand(leftHand, grabbedPointLeft, Vector3.zero);

                    // Sağ el biraz sağa offset'li (örneğin x ekseninde 0.2f birim kaydırıyoruz)
                    grabbedPointRight = point;
                    AttachHand(rightHand, grabbedPointRight, new Vector3(0, 0.2f, 0));
                    grabbedObject.ReserveGrabPoint(grabbedPointRight);

                    break;
                }
            }
        }
    }

    void AttachHand(Transform hand, Transform point)
    {
        hand.position = point.position;
        hand.rotation = point.rotation;
        FixedJoint joint = hand.gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = grabbedObject.GetComponent<Rigidbody>();
    }

    void Release()
    {
        if (grabbedPointLeft != null) grabbedObject.ReleaseGrabPoint(grabbedPointLeft);
        if (grabbedPointRight != null) grabbedObject.ReleaseGrabPoint(grabbedPointRight);

        Destroy(leftHand.GetComponent<FixedJoint>());
        Destroy(rightHand.GetComponent<FixedJoint>());

        grabbedObject = null;
        grabbedPointLeft = null;
        grabbedPointRight = null;
    }
}
