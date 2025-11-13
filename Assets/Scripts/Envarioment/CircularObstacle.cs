using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircularObstacle : MonoBehaviour
{
    public float rotationSpeed = 30f; // Derece/saniye


    private void Start()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void Update()
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<StarterAssets.ThirdPersonController>())
        {
            Debug.Log("Karakter tuzağa çarptı!");
            GameManager.instance.TriggerGameOver(); // Game Over'ı tetikle
        }
    }

    
}
