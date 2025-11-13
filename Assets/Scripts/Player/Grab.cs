using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input System
using Unity.Netcode; // Eğer multiplayer ortamdaysa
using System;

public class GrabNewInput : NetworkBehaviour
{
    public Animator animator;
    private GameObject grabbedObj;
    public Rigidbody rb; // Elin Rigidbody'si
    [Tooltip("0 = Sol El, 1 = Sağ El")]
    public int isLeftorRight; 
    public bool alreadyGrabbing = false;

    private InputAction grabAction; // Yeni input action

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // --- Input Action'ı manuel tanımla ---
        grabAction = new InputAction(
            name: isLeftorRight == 0 ? "LeftGrab" : "RightGrab",
            type: InputActionType.Button
        );

        grabAction.AddBinding(isLeftorRight == 0 ? "<Mouse>/leftButton" : "<Mouse>/rightButton");

// Gamepad desteği ekle
        grabAction.AddBinding(isLeftorRight == 0 ? "<Gamepad>/leftTrigger" : "<Gamepad>/rightTrigger");
        grabAction.AddBinding(isLeftorRight == 0 ? "<Gamepad>/buttonWest" : "<Gamepad>/buttonSouth"); 


        grabAction.performed += OnGrabPressed;
        grabAction.canceled += OnGrabReleased;

        grabAction.Enable();
    }

    private void OnDestroy()
    {
        grabAction.performed -= OnGrabPressed;
        grabAction.canceled -= OnGrabReleased;
        grabAction.Disable();
    }

    private void OnGrabPressed(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        if (alreadyGrabbing) return; // sadece bu kontrol yeterli

        if (grabbedObj != null)
        {
            alreadyGrabbing = true;

            // Animasyonu başlat
            if (isLeftorRight == 0)
                animator.SetBool("isLeftHandUp", true);
            else
                animator.SetBool("isRightHandUp", true);

            // Joint oluştur
            FixedJoint fj = grabbedObj.AddComponent<FixedJoint>();
            fj.connectedBody = rb;
            fj.breakForce = 9001;
        }
    }


    private void OnGrabReleased(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        if (!alreadyGrabbing) return;

        alreadyGrabbing = false;

        if (isLeftorRight == 0)
            animator.SetBool("isLeftHandUp", false);
        else
            animator.SetBool("isRightHandUp", false);

        if (grabbedObj != null)
        {
            FixedJoint fj = grabbedObj.GetComponent<FixedJoint>();
            if (fj != null)
                Destroy(fj);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item") && !alreadyGrabbing)
        {
            grabbedObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == grabbedObj && !alreadyGrabbing)
        {
            grabbedObj = null;
        }
    }
}
