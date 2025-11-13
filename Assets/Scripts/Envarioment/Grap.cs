using System;
using UnityEngine;

public class Grap : MonoBehaviour
{
  
   
    public Transform handTransform;
    private FixedJoint grabJoint;
    private Rigidbody grabbedRb;
    private Transform grabPoint;
    private Grabbable grabbedGrabbable;

    void Start()
    {
        if (handTransform == null)
        {
            handTransform = transform;
        }
    }

    void Update()
    {
        if (grabbedRb != null && Input.GetKeyDown(KeyCode.E))
        {
            ReleaseObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item") && grabJoint == null)
        {
            GrabObject(other.gameObject);
        }
    }

    private void GrabObject(GameObject obj)
    {
        grabbedGrabbable = obj.GetComponent<Grabbable>();
        if (grabbedGrabbable != null)
        {
            grabPoint = grabbedGrabbable.GetClosestAvailableGrabPoint(handTransform.position);
            if (grabPoint != null)
            {
                grabbedGrabbable.ReserveGrabPoint(grabPoint); // Noktayı rezerve et

                grabbedRb = obj.GetComponent<Rigidbody>();
                grabbedRb.useGravity = false;
                grabbedRb.isKinematic = false;

                // **Nesneyi sabitle**
                handTransform.position = grabPoint.position;
                handTransform.rotation = grabPoint.rotation;

                grabJoint = handTransform.gameObject.AddComponent<FixedJoint>();
                grabJoint.connectedBody = grabbedRb;
                grabJoint.breakForce = float.MaxValue;
                grabJoint.breakTorque = float.MaxValue;
            }
        }
    }

    private void ReleaseObject()
    {
        if (grabJoint != null)
        {
            Destroy(grabJoint);
            grabJoint = null;
        }

        if (grabbedGrabbable != null)
        {
            grabbedGrabbable.ReleaseGrabPoint(grabPoint); // Noktayı tekrar kullanılabilir yap
        }

        grabbedRb.useGravity = true;
        grabbedRb = null;
        grabPoint = null;
    }
        }
    

