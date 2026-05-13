using UnityEngine;

public class EnemyFSMManager : StateMachineFlow
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
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected bool isBoss;
    public bool IsBoss => isBoss;
    public int CurrentHealth => currentHealth;
    public event System.Action OnDeath;

    [Header("Ataque")]
    [SerializeField] private Collider weaponCollider;
    [SerializeField] private float hitboxDuration = 0.3f;
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private int damage = 1;

    private float lastAttackTime = -Mathf.Infinity;
    protected void Awake()
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
        maxHealth = 2;
        currentHealth = maxHealth;
        isBoss = false;
    }


    public float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    public bool CanSeePlayer()
    {
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

        weaponCollider.enabled = true;
        Invoke(nameof(DisableCollider), hitboxDuration);
    }

    private void DisableCollider()
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
    public void TakeDamage(int attack)
    {
        currentHealth -= attack;

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
    }

}
