using System;
using UnityEngine;
using System.Collections.Generic;

public class LimbCollision : MonoBehaviour
{
   public PlayerController playerController;

   private void Start()
   {
      playerController = GameObject.FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
   }

   private void OnCollisionEnter(Collision other)
   {
      playerController.isGrounded = true;
   }
}
