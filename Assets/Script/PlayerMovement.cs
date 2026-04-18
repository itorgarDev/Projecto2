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
    [SerializeField] private GameObject pauseMenuCanvasScroll;
    [SerializeField] private GameObject pauseMenuCanvasOptions;
    [SerializeField] private GameObject pauseMenuCanvasAudio;
    [SerializeField] private GameObject pauseMenuCanvasVideo;
    [SerializeField] private GameObject pauseMenuCanvasControls;
    [SerializeField] private GameObject pauseMenuCanvasBrillo;


    [SerializeField] private Animator scrollAnimator;

    public Vector2 MoveInput => moveInput;
    public bool IsDashing => isDashing;
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [SerializeField] private float gravity = 40f;      // gravedad rápida
    [SerializeField] private float snapDistance = 1.2f;
    [SerializeField] private LayerMask groundMask;

    private float verticalVelocity = 0f;

    private static PlayerMovement instance;

    void Awake()
    { /*
        // 1. Si ya existe un Player y NO soy yo → destruirme
       if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 2. Si estamos en la escena 0 → destruir Player
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Destroy(gameObject);
            return;
        }

        // 3. Registrar este Player como el único
        instance = this;

        // 4. Hacerlo persistente SOLO si no estamos en escena 0
        DontDestroyOnLoad(gameObject);
        */

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

        // Reasignar menú al cambiar de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
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


   /* private void FindPauseMenu()
    {
        if (pauseMenuCanvas != null)
            return;

        PauseMenuBreaker marker = FindObjectOfType<PauseMenuBreaker>();
        if (marker == null)
        {
            Debug.LogWarning("No se encontró el menú de pausa instanciado (PauseMenuBreaker).");
            return;
        }

        pauseMenuCanvas = marker.gameObject;

        // Canvas
        Transform canvas = pauseMenuCanvas.transform.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("No se encontró un hijo llamado 'Canvas' dentro de MENU_FINAL.");
            return;
        }

        // PANEL SCROLL (tu nombre real)
        pauseMenuCanvasScroll = pauseMenuCanvas.transform.Find("PanelScroll")?.gameObject;

        // PANEL OPTIONS (dentro de PanelScroll)
        pauseMenuCanvasOptions = pauseMenuCanvas.transform.Find("PanelScroll/PanelOptions")?.gameObject;

        // PANEL VIDEO (tu nombre real)
        pauseMenuCanvasVideo = pauseMenuCanvas.transform.Find("PanelVideo")?.gameObject;

        // PANEL BRILLO (tu nombre real)
        pauseMenuCanvasAudio = pauseMenuCanvas.transform.Find("PanelBrillo")?.gameObject;

        // Animator dentro de Scroll
        Transform scrollObj = pauseMenuCanvas.transform.Find("PanelScroll/Scroll");
        if (scrollObj != null)
            scrollAnimator = scrollObj.GetComponent<Animator>();

        Debug.Log($"Menú asignado. Scroll: {pauseMenuCanvasScroll != null}, Options: {pauseMenuCanvasOptions != null}, Video: {pauseMenuCanvasVideo != null}, Animator: {scrollAnimator != null}");
    }


    */

    private void FindPauseMenu()
{
    if (pauseMenuCanvas != null)
        return;

    PauseMenuBreaker marker = FindObjectOfType<PauseMenuBreaker>();
    if (marker == null)
    {
        Debug.LogWarning("No se encontró el menú de pausa instanciado (PauseMenuBreaker).");
        return;
    }

    pauseMenuCanvas       = marker.gameObject;
    pauseMenuCanvasScroll = marker.panelScroll;
    pauseMenuCanvasOptions= marker.panelOptions;
    pauseMenuCanvasVideo  = marker.panelVideo;
    pauseMenuCanvasAudio  = marker.panelAudio;
    pauseMenuCanvasBrillo = marker.panelBrillo;
    scrollAnimator        = marker.scrollAnimator;

    Debug.Log($"Menú asignado. Scroll: {pauseMenuCanvasScroll != null}, Options: {pauseMenuCanvasOptions != null}, Video: {pauseMenuCanvasVideo != null}, Animator: {scrollAnimator != null}");
}

    private void Start()
    {
        isPaused = false;
        Time.timeScale = 1;
        currentHealth = maxHealth;
        FindPauseMenu();
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
        // Si por lo que sea no se ha encontrado el menú, no sigas
        if (pauseMenuCanvas == null)
        {
            Debug.LogError("OnPausePerformed llamado pero pauseMenuCanvas es NULL. Revisa PauseMenuBreaker y la jerarquía de MENU_FINAL.");
            FindPauseMenu();
            if (pauseMenuCanvas == null) return;
        }

        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;

            if (pauseMenuCanvasScroll != null)
                pauseMenuCanvasScroll.SetActive(true);

            if (pauseMenuCanvasOptions != null)
                pauseMenuCanvasOptions.SetActive(true);

            pauseMenuCanvas.SetActive(true);

            if (scrollAnimator != null)
                scrollAnimator.SetTrigger("Scroll_Animation");
            else
                Debug.LogWarning("scrollAnimator es NULL, no se puede lanzar la animación.");

            Debug.Log("Menu abierto");
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1;
            HidePanels();

            if (pauseMenuCanvasOptions != null)
                pauseMenuCanvasOptions.SetActive(false);

            if (pauseMenuCanvasScroll != null)
                pauseMenuCanvasScroll.SetActive(false);

            pauseMenuCanvas.SetActive(false);

            Debug.Log("Menu cerrado");
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

        StartCoroutine(ResetTime());
        Debug.Log("Menu cerrado");
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

    void OnEnable()
    {
        controls.Enable();
        FindPauseMenu();   
    }
//    void OnEnable() => controls.Enable();
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

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
*/
    private void HandleVerticalMovement()
    {
        // 1. Aplicar gravedad arcade
        verticalVelocity -= gravity * Time.fixedDeltaTime;

        // 2. Raycast largo para detectar suelo
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100f, groundMask))
        {
            // 3. Snapping suave si estás cerca del suelo
            if (hit.distance <= snapDistance)
            {
                verticalVelocity = -2f; // pegado estable sin aplastar
            }
        }

        // 4. Aplicar velocidad vertical al rigidbody
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
