using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 4;
    [SerializeField] private float rotationSpeed = 10;

    [SerializeField] private float dashDistance = 8f;
    [SerializeField] private float dashCooldown = 3f;
    private float dashDuration = 0.2f;
    public bool IsImmortal { get; private set; }
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity; // hacemos que el dash esté disponible al inicio dando un valor negativo grande

    // Vectores fijos para isométrico clásico
    private readonly Vector3 forward = new Vector3(1, 0, 1).normalized;
    private readonly Vector3 right = new Vector3(1, 0, -1).normalized;

    public float horizontalInput;

    void Update()
    {
        if (isDashing) return; // Bloquea todo mientras dura el dash

        horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = horizontalInput * right + verticalInput * forward;
        
        if (direction.magnitude > 0.1f) // Evita movimientos muy pequeños
        {
            Vector3 normalizedDirection = direction.normalized; // Normaliza la dirección para mantener una velocidad constante
            transform.position += normalizedDirection * speed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(normalizedDirection); 
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Suaviza la rotación hacia la dirección del movimiento
        }
        ///////
        ///             AREGLAR DELAY AL DEJAR DE PULSAR TECLA MOV. MOTIVO=> DE -1 A 1 INCREMENTANDO EL VALOR PCOCO A POCO EN VEZ DE DE 0 A 1 DIRECTAMENTE 
        ///////


        if (Input.GetKeyDown(KeyCode.Space) && direction.magnitude > 0.1f && Time.time - lastDashTime >= dashCooldown && !isDashing) // Inicia el dash si se presiona espacio, hay dirección y el cooldown ha pasado
        {
            StartCoroutine(DashCoroutine(direction.normalized));
        }
    }

    private IEnumerator DashCoroutine(Vector3 dashDirection) // Corutina para manejar el dash
    {
        isDashing = true;
        IsImmortal = true; // El jugador es inmortal durante el dash
        lastDashTime = Time.time; // Actualiza el tiempo del último dash

        float elapsed = 0f; 
        Vector3 start = transform.position;
        Vector3 end = start + dashDirection * dashDistance;

        while (elapsed < dashDuration) // si dashduration es mas chico que el tiempo de frame puede que no llegue exactamente al final y cause efectos raros (no tocar)
        {
            float t = elapsed / dashDuration; // divide el tiempo transcurrido por la duración total para obtener un valor entre 0 y 1
            transform.position = Vector3.Lerp(start, end, t); 
            elapsed += Time.deltaTime;
            yield return null;
        }


        transform.position = end;
        isDashing = false; // Vuelve a permitir movimiento
        IsImmortal = false; // El jugador ya no es inmortal
    }
}
