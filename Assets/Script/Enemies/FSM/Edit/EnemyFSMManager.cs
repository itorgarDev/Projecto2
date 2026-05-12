using UnityEngine;

public class EnemyFSMManager : StateMachineFlow
{
    // ============================
    //        ESTADOS
    // ============================
    public Idle idleState;
    public Chase chaseState;
    public Attack attackState;

    // ============================
    //        MOVIMIENTO
    // ============================
    [Header("Movimiento")]
    public float chaseSpeed = 4f;
    public float stopDistance = 6f;
    public float attackDistance = 2f;

    // ============================
    //        VISIÓN
    // ============================
    [Header("Visión")]
    public float visionRange = 8f;
    public float visionAngle = 45f;
    public LayerMask obstacleMask;

    // ============================
    //        COMPONENTES
    // ============================
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Transform player;
    private PlayerStats playerStats;

    [Header("Animadores")]
    public Animator animator;
    public Animator animatorExclamation;

    // ============================
    //        VIDA
    // ============================
    [Header("Vida")]
    [SerializeField] private int maxHealth = 2;
    private int currentHealth;
    [SerializeField] private bool isBoss = false;
    public bool IsBoss => isBoss;

    // ============================
    //        ATAQUE
    // ============================
    [Header("Ataque")]
    [SerializeField] private Collider weaponCollider;
    [SerializeField] private float hitboxDuration = 0.3f;
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private int damage = 1;

    private float lastAttackTime = -Mathf.Infinity;

    // ============================
    //        AWAKE
    // ============================
    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        idleState = new Idle(this);
        chaseState = new Chase(this);
        attackState = new Attack(this);

        weaponCollider.enabled = false;
        currentHealth = maxHealth;
    }

    // ============================
    //        FSM
    // ============================
    protected override void GetInitialState(out TemplateStateMachine _stateMachine)
    {
        _stateMachine = idleState;
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

    // ============================
    //        ATAQUE
    // ============================
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

    // ============================
    //        VIDA
    // ============================
    public void TakeDamage(int attack)
    {
        currentHealth -= playerStats.attack;

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
        EnemyPool.Instance.ReturnToPool(GetComponent<Enemy>());
    }
}
