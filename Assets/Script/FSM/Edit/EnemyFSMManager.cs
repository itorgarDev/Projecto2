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
    [HideInInspector] public EnemyAttack enemyAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAttack = GetComponent<EnemyAttack>();

        idleState = new Idle(this);
        chaseState = new Chase(this);
        attackState = new Attack(this);
    }


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
}
