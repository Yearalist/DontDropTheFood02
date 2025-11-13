using UnityEngine;

using Unity.Netcode;

using UnityEngine.InputSystem; // YENİ: Input System'i dahil et



// PlayerInput bileşeninin bu objede olmasını zorunlu kıl

[RequireComponent(typeof(PlayerInput))]

public class RagdolPlayerController : NetworkBehaviour

{

public float speed;

public float strafeSpeed;

public float jumpForce = 5000f;


public Rigidbody hips;


[Header("Durum (Sadece İzleyin)")]

public bool isGrounded = true;



// --- Yeni Input Değişkenleri ---

private Vector2 moveInput; // Gamepad'in veya WASD'nin (X, Y) değerini tutar

private bool jumpInput_isPressed = false; // Zıplama isteğini tutar

private bool runInput_isPressed = false; // Koşma isteğini tutar




// PlayerInput (Behavior: Send Messages) tarafından otomatik çağrılır

public void OnMove(InputValue value)

{

// Sadece sahip olan input alabilir

if (!IsOwner) return;

moveInput = value.Get<Vector2>();

}

public override void OnNetworkSpawn()
{
    if (!IsOwner)
    {
        // Sadece sahibi olan oyuncu input alabilsin
        var input = GetComponent<PlayerInput>();
        if (input != null)
            input.enabled = false;
    }
}




// PlayerInput tarafından otomatik çağrılır

public void OnJump(InputValue value)

{

if (!IsOwner) return;


// Sadece tuşa basıldığı an ve yerdeysek zıplama isteği oluştur

if (value.isPressed && isGrounded)

{

jumpInput_isPressed = true;

}

}



// PlayerInput tarafından otomatik çağrılır

public void OnRun(InputValue value)

{

if (!IsOwner) return;

runInput_isPressed = value.isPressed;

}





// Update() metoduna artık ihtiyacımız yok, çünkü input'u eventlerle alıyoruz.




private void FixedUpdate()

{

// Fiziği SADECE sahip olan uygular

if (!IsOwner) return;


// --- Hareket Kodları (Yeni Input Değerlerini Kullanan) ---



// moveInput.y -> Dikey eksen (W = +1, S = -1)

// moveInput.x -> Yatay eksen (D = +1, A = -1)



// İleri (W veya Gamepad Stick Yukarısı)

if (moveInput.y > 0.1f)

{

if (runInput_isPressed) // Koşma (Shift veya Stick Basma)

{

// moveInput.y ile çarparak analog hassasiyeti ekliyoruz

hips.AddForce(hips.transform.forward * speed * 1.5f * moveInput.y);

}

else

{

hips.AddForce(hips.transform.forward * speed * moveInput.y);

}

}


// Geri (S veya Gamepad Stick Aşağısı)

if (moveInput.y < -0.1f)

{

// Mathf.Abs, değeri pozitife çevirir (çünkü moveInput.y negatiftir)

hips.AddForce(-hips.transform.forward * strafeSpeed * Mathf.Abs(moveInput.y));

}



// Sol (A veya Gamepad Stick Sol)

if (moveInput.x < -0.1f)

{

hips.AddForce(-hips.transform.right * strafeSpeed * Mathf.Abs(moveInput.x));

}



// Sağ (D veya Gamepad Stick Sağ)

if (moveInput.x > 0.1f)

{

hips.AddForce(hips.transform.right * strafeSpeed * moveInput.x);

}




// --- Zıplama Kodu ---

if (jumpInput_isPressed)

{

hips.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

isGrounded = false;

jumpInput_isPressed = false; // İsteği yerine getirdik, sıfırla

}

}

}