using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 4;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float maxForce = 20f;

    [SerializeField] private float dashDistance = 8f;
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] private ParticleSystem dashWind;

    private bool dashRequested = false;
    private Vector3 dashDir;
    [SerializeField] private LayerMask obstacleMask;


    public bool IsImmortal { get; private set; }
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity; // hacemos que el dash esté disponible al inicio dando un valor negativo grande

    // Vectores fijos para isométrico clásico
    private readonly Vector3 forward = new Vector3(1, 0, 1).normalized;
    private readonly Vector3 right = new Vector3(1, 0, -1).normalized;

    private Rigidbody rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private TakeDrop currentItem;
    private DialogueSystem currentNpc;
    private bool isPaused = false;
    private PlayerAttack playerAttack;
    [SerializeField] private GameObject pauseMenuCanvas;
    public Vector2 MoveInput => moveInput;
    public bool IsDashing => isDashing;
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    


    void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();

        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Dash.performed += OnDashPerformed;
        controls.Player.Take.performed += OnTakePerformed;
        controls.Player.Pause.performed += OnPausePerformed;
        controls.Player.Respawn.performed += OnRespawnPerformed;
        controls.Player.Interact.performed += OnInteractPerformed;

        playerAttack = GetComponentInChildren<PlayerAttack>();
        controls.Player.Attack.performed += OnAttackPerformed;

    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnRespawnPerformed(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (!isPaused)  // esto funciona como interruptor dependiendo del valor de isPaused
        {
            isPaused = true; // al poner ! isPaused indica el valor contrario
            Time.timeScale = 0; // entonces si entra en false se vuelve true y viceversa creando asi alternancia para activar y desactivar la pausa
            pauseMenuCanvas.SetActive(true);
        }                       
        else
        {
            isPaused = false;      
            Time.timeScale = 1;
            pauseMenuCanvas.SetActive(false);
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    private void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        Vector3 direction = moveInput.x * right + moveInput.y * forward;

        if (direction.magnitude > 0.1f && Time.time - lastDashTime >= dashCooldown && !isDashing)
        {
            dashDir = direction.normalized;   // guardamos la dirección del dash
            dashRequested = true;             // marcamos que se ha pedido un dash
        }
        Debug.Log("DASH INPUT");

    }

    private void OnTakePerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && currentItem != null)
        {
            currentItem.PickUp();
            currentItem = null;
        }
    }
    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        // si dialogo esta activo continua
        if (currentNpc != null && currentNpc.IsDialogueActive)
        {
            currentNpc.ContinueDialogue();
            return;
        }

        // si no esta activo pero tenemos un npc al alcance lo empieza
        if (currentNpc != null)
        {
            currentNpc.StartDialogue();
            return;
        }
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && playerAttack != null)
        {
            playerAttack.PerformAttack();
        }
    }

    public void TakeDamage(int amount)
    {
        if (IsImmortal) return; // no recibe daño si está en dash

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // reaparece en el último checkpoint
        transform.position = RespawnSystem.LastCheckpointPos;

        // reinicia la vida
        currentHealth = maxHealth;

        // resetea estados
        isDashing = false;
        IsImmortal = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    void FixedUpdate()
    {
        if (dashRequested)
        {
            dashRequested = false;
            StartCoroutine(DashCoroutine(dashDir));
            return;
        }

        if (isDashing) return;

        // Dirección isométrica fija
        Vector3 direction = moveInput.x * right + moveInput.y * forward;

        // Velocidad objetivo
        Vector3 targetVelocity = direction * speed;

        // Física
        Vector3 currentVelocity = rb.velocity;
        Vector3 velocityChange = targetVelocity - currentVelocity;

        // Limitar fuerza máxima
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

        // Aplicar movimiento físico
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        // Rotación del personaje hacia la dirección de movimiento
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
        if (dashRequested)
            Debug.Log("DASH REQUESTED");


    }

    private IEnumerator DashCoroutine(Vector3 dashDirection) // Corutina para manejar el dash
    {
        Debug.Log("DASH START");
        dashWind.Play();

        isDashing = true;
        IsImmortal = true; // El jugador es inmortal durante el dash
        lastDashTime = Time.time; // Actualiza el tiempo del último dash

        float elapsed = 0f;
        float dashSpeed = dashDistance / dashDuration; // velocidad necesaria para recorrer la distancia exacta

        while (elapsed < dashDuration)
        {
            float step = dashSpeed * Time.fixedDeltaTime;
            Vector3 nextPos = rb.position + dashDirection * step;

            /*// comprobamos si entre la posición actual y la siguiente hay algo sólido
            if (Physics.Raycast(rb.position, dashDirection, out RaycastHit hit, step + 0.1f, obstacleMask))
            {
                break; // solo se detiene si lo que hay delante es una pared
            }*/


            rb.MovePosition(nextPos);

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }


        isDashing = false; // Vuelve a permitir movimiento
        IsImmortal = false; // El jugador ya no es inmortal
        dashWind.Stop();


    }

    private void OnTriggerStay(Collider other) 
    {
        //detecta items
        if (other.TryGetComponent<TakeDrop>(out TakeDrop item))
        {
            currentItem = item; // actualiza el valor de currentitem si esta dentro del triger
        }

        //detecta npcs
        if (other.TryGetComponent<DialogueSystem>(out DialogueSystem npcDialogue)) 
        { 
            currentNpc = npcDialogue; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<TakeDrop>(out TakeDrop item) && item == currentItem)
        {
            currentItem = null; // actualiza el valor de currentitem si YA NO  esta dentro del triger
        }

        if (other.TryGetComponent<DialogueSystem>(out DialogueSystem npcDialogue) && npcDialogue == currentNpc) 
        { 
            currentNpc = null; // lo mismo con npcs

            // esto hace que los mensajes desaparezcan y se reseteen si se aleja del npc
            if (npcDialogue.IsDialogueActive) npcDialogue.EndDialogue();
        }
    }
}
