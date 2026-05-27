using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

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
    private BridgeSwap currentBridge;
    private bool isPaused = false;
    private PlayerAttack playerAttack;
    private PlayerStats stats;


    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject pauseMenuCanvasScroll;
    [SerializeField] private GameObject pauseMenuCanvasOptions;
    [SerializeField] private GameObject pauseMenuCanvasAudio;
    [SerializeField] private GameObject pauseMenuCanvasVideo;
    [SerializeField] private GameObject pauseMenuCanvasControls;
    [SerializeField] private GameObject pauseMenuCanvasBrillo;
    [SerializeField] private GameObject pauseMenuCanvasOscuro;

    [SerializeField] private GameObject canvasMapa;
    [SerializeField] private GameObject canvasE;

    [SerializeField] private GameObject panelDeath;

    [SerializeField] private Animator scrollAnimator;

    public Vector2 MoveInput => moveInput;
    public bool IsDashing => isDashing;
    
    [SerializeField] private float gravity = 40f;      // gravedad rápida
    [SerializeField] private float snapDistance = 1.2f;
    [SerializeField] private LayerMask groundMask;

    private float verticalVelocity = 0f;

    bool isMapOpen = false;
    private bool isWalking = false;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private GameObject playerPrefab;



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

        stats = GetComponent<PlayerStats>();

        // Reasignar menú al cambiar de escena
        SceneManager.sceneLoaded += OnSceneLoaded;

        controls.Player.Map.performed += OnMapPerformed;

        

    }

    private void Start()
    {
        isPaused = false;
        Time.timeScale = 1;
        FindPauseMenu();
        stats.currentHealth = stats.maxHealth;

    

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Esperar un frame para que MENU_FINAL(Clone) aparezca en la jerarquía
        StartCoroutine(DelayedFindMenu());
    }

    private IEnumerator DelayedFindMenu()
    {
        yield return null; // esperar 1 frame
        FindPauseMenu();
    }
           
    private void FindPauseMenu()
    {
    }

   

    private void OnRespawnPerformed(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void HidePanels()
    {
        if (pauseMenuCanvasAudio != null)
            pauseMenuCanvasAudio.SetActive(false);

        if (pauseMenuCanvasVideo != null)
            pauseMenuCanvasVideo.SetActive(false);

        if (pauseMenuCanvasControls != null)
            pauseMenuCanvasControls.SetActive(false);
    }



    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (isMapOpen) return;
        if (pauseMenuCanvas == null)
        {
            FindPauseMenu();
            if (pauseMenuCanvas == null) return;
        }

        if (!isPaused)
        {
            isPaused = true;
            audioMixer.SetFloat("SFXVolume", -80f);
            Time.timeScale = 0;

            pauseMenuCanvas.SetActive(true);
            pauseMenuCanvasOscuro?.SetActive(true);
            pauseMenuCanvasScroll?.SetActive(true);
            pauseMenuCanvasOptions?.SetActive(true);

            // 🔹 Mantén el brillo activo
            pauseMenuCanvasBrillo?.SetActive(true);

            scrollAnimator?.SetTrigger("Scroll_Animation");
            Debug.Log("Menú abierto");
        }
        else
        {
            isPaused = false;
            audioMixer.SetFloat("SFXVolume", 0f);
            HidePanels();
            Time.timeScale = 1;

            pauseMenuCanvasOscuro?.SetActive(false);
            pauseMenuCanvasScroll?.SetActive(false);
            pauseMenuCanvasOptions?.SetActive(false);

            // 🔹 NO desactives el brillo
            // pauseMenuCanvasBrillo?.SetActive(false);

            pauseMenuCanvas?.SetActive(false);
            Debug.Log("Menú cerrado");
        }
    }

    public void Transport()
    {
        Time.timeScale = 1f;
        transform.position = RespawnSystem.LastCheckpointPos + new Vector3(-2, 0, -2);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        pauseMenuCanvas.SetActive(false);
        pauseMenuCanvasOptions.SetActive(true);
        pauseMenuCanvasScroll.SetActive(false);
        Debug.Log("Menu cerrado");
    }

    private IEnumerator ResetTime()
    {
        yield return null; // esperar 1 frame
        Time.timeScale = 1f;
    }

    public void ClosePauseMenu()
    {
        isPaused = false;
        pauseMenuCanvas.SetActive(false);
        pauseMenuCanvasOptions.SetActive(true);
        pauseMenuCanvasScroll.SetActive(false);
        pauseMenuCanvasBrillo.SetActive(true);
        StartCoroutine(ResetTime());
        Debug.Log("Menu cerrado");
    }


    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        if (isMapOpen) return;
        moveInput = ctx.ReadValue<Vector2>();
        if (!isWalking)
        {
            isWalking = true;
            SoundController.Instance.PlaySFX(SoundController.Instance.walkSfx);
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
        isWalking=false;    
    }

    private void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        if (isMapOpen) return;
        Vector3 direction = moveInput.x * right + moveInput.y * forward;

        if (direction.magnitude > 0.1f && Time.time - lastDashTime >= dashCooldown && !isDashing)
        {
            dashDir = direction.normalized;   // guardamos la dirección del dash
            dashRequested = true;             // marcamos que se ha pedido un dash

            SoundController.Instance.PlaySFX(SoundController.Instance.dashSfx);

        }
        Debug.Log("DASH INPUT");

    }

    private void OnMapPerformed(InputAction.CallbackContext context)
    {
        if (canvasMapa == null)
        {
            Debug.LogWarning("CanvasMapa no asignado en el inspector.");
            return;
        }

        if (isPaused) return;

        // Alternar estado REAL del mapa
        isMapOpen = !isMapOpen;

        canvasMapa.SetActive(isMapOpen);
        Time.timeScale = isMapOpen ? 0f : 1f;

        Debug.Log(isMapOpen ? "Mapa abierto" : "Mapa cerrado");
    }


    private void OnTakePerformed(InputAction.CallbackContext ctx)
    {
        if (isMapOpen) return;
        if (ctx.performed && currentItem != null)
        {
            currentItem.PickUp();
            currentItem = null;
            SoundController.Instance.PlaySFX(SoundController.Instance.item);
        }
    }
    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        if (isMapOpen) return;
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

        if (currentBridge != null)
        {
            currentBridge.InteractWithBridge();
            return;
        }
    }

    void OnEnable()
    {
        controls.Enable();
        FindPauseMenu();   
    }
//    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (isMapOpen) return;
        if (isPaused) return;
        if (ctx.performed && playerAttack != null)
        {
            playerAttack.PerformAttack();
            SoundController.Instance.PlaySFX(SoundController.Instance.attackSfx);
        }
    }

    public void TakeDamage(int amount)
    {
        if (IsImmortal) return;

        stats.TakeDamage(amount);
        SoundController.Instance.PlaySFX(SoundController.Instance.damageSfx);

        if (stats.currentHealth <= 0)
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.deathSfx);
            GameOver();
            
            
        }
    }

    public void GameOver()
    {
        // Congelar el juego

        Time.timeScale = 0f;
        // Desactivar movimiento y ataques
        controls.Disable();
        rb.velocity = Vector3.zero;

        // Mostrar panel de muerte
        panelDeath.SetActive(true);
    }

    public void Die()
    {
        // Reactivar tiempo
        Time.timeScale = 1f;

        // Ocultar panel
        panelDeath.SetActive(false);

        // Mover al checkpoint
        transform.position = RespawnSystem.LastCheckpointPos;

        // Resetear físicas
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Restaurar vida
        stats.currentHealth = stats.maxHealth;
        HUDController.Instance.UpdateHealthBar();

        // Reactivar controles
        controls.Enable();


    }


    
    private void HandleVerticalMovement()
    {
        // Aplicar gravedad arcade
        verticalVelocity -= gravity * Time.fixedDeltaTime;

        // Raycast largo para detectar suelo
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100f, groundMask))
        {
            // Snapping suave si estás cerca del suelo
            if (hit.distance <= snapDistance)
            {
                verticalVelocity = -2f; // pegado estable sin aplastar
            }
        }

        // Aplicar velocidad vertical al rigidbody
        rb.velocity = new Vector3(rb.velocity.x, verticalVelocity, rb.velocity.z);
    }



    void FixedUpdate()
    {
        if (dashRequested)
        {
            dashRequested = false;
            StartCoroutine(DashCoroutine(dashDir));
            return;
        }

        if (isDashing)
        {
            // Durante el dash: gravedad arcade sin snapping
            verticalVelocity -= gravity * Time.fixedDeltaTime;
            rb.velocity = new Vector3(rb.velocity.x, verticalVelocity, rb.velocity.z);
            return;
        }

        // Dirección isométrica fija
        Vector3 direction = moveInput.x * right + moveInput.y * forward;

        // Velocidad objetivo
        Vector3 targetVelocity = direction * speed;

        // Física
        Vector3 currentVelocity = rb.velocity;
        Vector3 velocityChange = targetVelocity - new Vector3(currentVelocity.x, 0, currentVelocity.z);

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

        // gravedad + snapping arcade
        HandleVerticalMovement();
    }

    void Update()
    {
        // Calculamos cuánto tiempo ha pasado desde el último dash
        float timeTranscurred = Time.time - lastDashTime;

        // Le decimos al HUD: "Oye, actualiza la ola con este tiempo"
        if (HUDController.Instance != null)
        {
            HUDController.Instance.UpdateDashCooldown(timeTranscurred, dashCooldown);
        }

        if (currentItem == null && currentNpc == null)
        {
            if (canvasE.activeSelf)
                canvasE.SetActive(false);
        }

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

        if (playerAttack != null && playerAttack.IsAttacking)
        {
            playerAttack.ForceCancelAttack();
        }

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
        canvasE.SetActive(true);
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

        if (other.TryGetComponent<BridgeSwap>(out BridgeSwap bridge))
        {
            currentBridge = bridge;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canvasE.SetActive(false);
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

        if (other.TryGetComponent<BridgeSwap>(out BridgeSwap bridge) && bridge == currentBridge)
        {
            currentBridge = null;
        }
    }
}
