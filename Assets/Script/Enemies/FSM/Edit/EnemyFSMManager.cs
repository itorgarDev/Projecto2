using UnityEngine;

public class EnemyFSMManager : StateMachineFlow, IDamageable
{
    public Idle idleState;
    public Chase chaseState;
    public Attack attackState;
    public Death deathState; 

    [Header("Movimiento")]
    public float chaseSpeed = 4f;
    public float stopDistance = 6f;
    public float attackDistance = 2f;

    [Header("Visión")]
    public float visionRange = 8f;
    public float visionAngle = 45f;
    public LayerMask obstacleMask;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Transform player;
    private PlayerStats playerStats;

    [Header("Animadores")]
    public Animator animator;
    public Animator animatorExclamation;

    [Header("Vida")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isBoss;
    public bool IsBoss => isBoss;
    public float CurrentHealth => currentHealth;
    public event System.Action OnDeath;

    [Header("Ataque")]
    [SerializeField] public Collider weaponCollider;
    [SerializeField] public float hitboxDuration = 0.3f;
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private int damage = 1;

    private float lastAttackTime = -Mathf.Infinity;

    public enum EnemySoundType
    {
        Normal,
        Xuanwu
    }

    public EnemySoundType soundType;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }

        idleState = new Idle(this);
        chaseState = new Chase(this);
        attackState = new Attack(this);
        deathState = new Death(this); 

        weaponCollider.enabled = false;
        InitializeStats();
    }

    // Esto se ejecuta CADA VEZ que el Pool saca al enemigo de nuevo a la escena
    private void OnEnable()
    {
        InitializeStats();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }

        if (TryGetComponent<Collider>(out var mainCollider))
        {
            mainCollider.enabled = true;
        }

        if (weaponCollider != null) weaponCollider.enabled = false;

     
        if (idleState != null)
        {
            ChangeState(idleState);
        }
    }

    protected override void GetInitialState(out TemplateStateMachine _stateMachine)
    {
        _stateMachine = idleState;
    }

    protected virtual void InitializeStats()
    {
        maxHealth = 2f;
        currentHealth = maxHealth;
        isBoss = false;
    }

    public float DistanceToPlayer()
    {
        if (Time.timeScale == 0f) return Mathf.Infinity;
        if (player == null) return Mathf.Infinity;

        return Vector3.Distance(transform.position, player.position);
    }

    public bool CanSeePlayer()
    {
        if (Time.timeScale == 0f) return false;
        if (player == null) return false;

        Vector3 dir = (player.position - transform.position).normalized;
        float dist = DistanceToPlayer();

        if (Vector3.Angle(transform.forward, dir) < visionAngle &&
            dist < visionRange &&
            !Physics.Raycast(transform.position + Vector3.up, dir, dist, obstacleMask))
        {
            return true;
        }

        return false;
    }

    public void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;
        PerformAttack();
    }

    private void PerformAttack()
    {
        animator.SetTrigger("Attack");

        switch (soundType)
        {
            case EnemySoundType.Xuanwu:
                SoundController.Instance.PlaySFX(SoundController.Instance.xMelee);
                break;

            case EnemySoundType.Normal:
                SoundController.Instance.PlaySFX(SoundController.Instance.cAttack);
                break;
        }
    }

    public void EnemyHit()
    {
        // Si ya está muerto, ignoramos cualquier evento de animación rezagado
        if (currentHealth <= 0) return;

        weaponCollider.enabled = true;
        Invoke(nameof(DisableCollider), hitboxDuration);
    }

    public void DisableCollider()
    {
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si está muerto, no puede hacer daño bajo ningún concepto
        if (currentHealth <= 0) return;

        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.TakeDamage(damage);
        }
    }

    public virtual void SystemTakeDamage(float amount)
    {
        if (currentHealth <= 0) return; // Si ya está muerto, no recibe más daño

        currentHealth -= amount;
        Debug.Log($"[{gameObject.name}] Daño recibido: {amount}. Vida restante: {currentHealth}");

       
        if (SoundController.Instance != null)
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.cDamage);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();

        // Cambiamos inmediatamente al estado de Muerte en la FSM para apagar la IA
        ChangeState(deathState);

        // Activar el Trigger de muerte en el Animator
        if (animator != null)
        {
            animator.SetTrigger("IsDead");
        }

        // Frenar por completo las físicas y quitar colisiones 
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        if (weaponCollider != null) weaponCollider.enabled = false;

        if (TryGetComponent<Collider>(out var mainCollider))
        {
            mainCollider.enabled = false;
        }

        if (SoundController.Instance != null)
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.cDeath);
        }

        // Esperamos los 2 segundos exactos para que termine de caer al suelo antes de ocultarlo
        Invoke(nameof(ReturnToPoolAfterDeath), 1.5f);
    }

    private void ReturnToPoolAfterDeath()
    {
        // Enviamos al pool original
        EnemyPool.Instance.ReturnToPool(this);
    }
}