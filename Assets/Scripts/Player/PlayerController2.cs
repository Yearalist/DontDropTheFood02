using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
     
    public float speed2;
    public float sidespeed2;
    public float jumpForce2;

    public Rigidbody hips2;
    public bool isGrounded2;

    private void Start()
    {
        hips2 = GetComponent<Rigidbody>();
        
    }
// fizik kullanacağım için kullandım 
    private void FixedUpdate()
    {
        
        if (Input.GetKey(KeyCode.I))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                hips2.AddForce(hips2.transform.forward * speed2 * .5f);
            }
            else
            {
                hips2.AddForce(hips2.transform.forward * speed2);
            }
            
        }

        if (Input.GetKey(KeyCode.J))
        {
            hips2.AddForce(-hips2.transform.right * sidespeed2);
        }
        if (Input.GetKey(KeyCode.K))
        {
            hips2.AddForce(-hips2.transform.forward*speed2);
        }
        if (Input.GetKey(KeyCode.L))
        {
            hips2.AddForce(hips2.transform.right * sidespeed2);
        }

        if (Input.GetAxis("Jump")> 0)
        {
            if (isGrounded2)
            {
                hips2.AddForce(new Vector3(0,jumpForce2,0));
                isGrounded2 = false;
            }
        }
    }
}
