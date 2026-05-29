using UnityEngine;

// Añadimos el contrato IDamageable aquí para que todos los enemigos comunes y jefes que hereden lo tengan
public class EnemyFSMManager : StateMachineFlow, IDamageable
{
    public Idle idleState;
    public Chase chaseState;
    public Attack attackState;

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
    // Cambiamos a float para que coincida perfectamente con el sistema del Fénix y el player
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isBoss;
    public bool IsBoss => isBoss;
    public float CurrentHealth => currentHealth; // Ahora devuelve float
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



    protected virtual void Awake() // Lo hacemos virtual por si el boss necesita sobreescribirlo limpiamente
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerStats = player.GetComponent<PlayerStats>();

        idleState = new Idle(this);
        chaseState = new Chase(this);
        attackState = new Attack(this);

        weaponCollider.enabled = false;
        InitializeStats();
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

        /*weaponCollider.enabled = true;
        Invoke(nameof(DisableCollider), hitboxDuration);*/
    }
    public void EnemyHit()
    {
        weaponCollider.enabled = true;
        Invoke(nameof(DisableCollider), hitboxDuration);
    }

    public void DisableCollider()
    {
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    // ¡REPARADO! Cambiado para cumplir el contrato IDamageable usando float
    public void SystemTakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"[{gameObject.name}] Daño recibido: {amount}. Vida restante: {currentHealth}");
        if (currentHealth < 0)
        {
            switch (soundType)
            {
                case EnemySoundType.Xuanwu:
                    SoundController.Instance.PlaySFX(SoundController.Instance.xDamage);
                    break;

                case EnemySoundType.Normal:
                    SoundController.Instance.PlaySFX(SoundController.Instance.cDamage);
                    break;
            }
        }
        

        if (isBoss)
        {
            var boss = GetComponent<BossEvokerFSMManager>();
            if (boss != null)
                boss.EvaluateWaves();
        }

        if (currentHealth <= 0)
            Die();
    }


    private void Die()
    {
        OnDeath?.Invoke();
        EnemyPool.Instance.ReturnToPool(this);
        switch (soundType)
        {
            case EnemySoundType.Xuanwu:
                SoundController.Instance.PlaySFX(SoundController.Instance.xMelee);
                break;

            case EnemySoundType.Normal:
                SoundController.Instance.PlaySFX(SoundController.Instance.cDeath);
                break;
        }
    }
}