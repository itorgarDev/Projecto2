using UnityEngine;

public class PhoenixFSM : StateMachineFlow, IDamageable
{
    public FlyState flyState;
    public ShootState shootState;
    public TransitionAirState transitionAirState;
    public GroundedState groundedState;
    public PhaseTransitionState phaseTransitionState;

    [Header("Variables internas del Fénix")]
    [Range(0f, 1f)]
    public float stamina = 1f;
    public float AirTime = 0f;
    public float GroundTime = 0f;
    public float PlayerDistance = 0f;
    public float GroundDamageRecieve = 0f;
    public int CurrentPhase = 1;
    public bool isGrounded = false; // nos dice si esta en el suelo de verdad o no
    public float maxShootRange = 30f;

    [Header("Referencias")]
    public Transform target;
    public GameObject bulletPrefab; 
    public Transform firePoint;

    [Header("Animación")]
    public Animator animator;

    [Header("Sistema de Vida")]
    public float Health = 15f;
    public float maxHealth = 15f;
    [HideInInspector] public bool isDead = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>(); 
    }

    protected override void GetInitialState(out TemplateStateMachine _stateMachine)
    {
        flyState = new FlyState("Fly", this);
        shootState = new ShootState("Shoot", this);
        transitionAirState = new TransitionAirState("TransitionAir", this);
        groundedState = new GroundedState("Grounded", this);
        phaseTransitionState = new PhaseTransitionState("PhaseTransition", this);

        _stateMachine = flyState;
    }

    // este metodo es el contrato q llama el player cuando pega
    public void SystemTakeDamage(float amount)
    {
        if (isDead) return;

        // no esta en el suelo, rechaza el daño
        if (!isGrounded)
        {
            Debug.Log("[Fenix] El boss esta volando o en el aire, no le haces daño.");
            return;
        }

        Health -= amount;
        Debug.Log("[Fenix] Daño recibido: " + amount + ". Vida actual: " + Health);

       

        GroundDamageRecieve += amount;

        // por si le metes el ultimo viaje en el suelo en fase 1
        if (Health <= 0f && CurrentPhase == 1)
        {
            ChangeState(phaseTransitionState);
            return;
        }

        if (Health <= 0f && CurrentPhase == 2)
        {
            Die();
        }
    }
    public void ResetAire()
    {
        AirTime = 0f;
        isGrounded = false; // Al resetear el aire, significa que ya NO está en el suelo
    }

    public void ResetTierra()
    {
        GroundTime = 0f;
        GroundDamageRecieve = 0f;
        isGrounded = true; // Al resetear el tierra, significa que ESTA en el suelo
    }
    // aqui paramos todo cuando el bicho la palme de verdad
    private void Die()
    {
        isDead = true;
        Debug.Log("[Fenix] El jefe a sido derrotado");
        Destroy(gameObject);
    }

}