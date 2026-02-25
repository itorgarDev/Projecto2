using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum AgentStates { idle, chase, attack }
    public AgentStates currentState;

    private EnemyAttack enemyAttack;

    [Header("Movimiento")]
    public float chaseSpeed = 4f;       // velocidad fija al perseguir
    public float stopDistance = 6f;   // distancia mínima para detenerse
    public float attackDistance = 2f;   // distancia para atacar


    [Header("Visión")]
    public float visionRange = 8f;
    public float visionAngle = 45f;
    public LayerMask obstacleMask;

    Rigidbody rb;
    Transform player;

    private void Start()
    {
        currentState = AgentStates.idle;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAttack = GetComponent<EnemyAttack>();

    }

    private void Update()
    {
        currentState = ChangeState();

        switch (currentState)
        {
            case AgentStates.idle:
                Idle();
                break;

            case AgentStates.chase:
                Chase();
                break;

            case AgentStates.attack:
                Attack();
                break;
        }
    }

    AgentStates ChangeState()
    {
        if (!CanSeePlayer())
            return AgentStates.idle;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackDistance)
            return AgentStates.attack;

        return AgentStates.chase;
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (Vector3.Angle(transform.forward, directionToPlayer) < visionAngle)
        {
            if (distanceToPlayer < visionRange)
            {
                if (!Physics.Raycast(transform.position + Vector3.up, directionToPlayer, distanceToPlayer, obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void Idle()
    {
        // Aquí puedes poner animación idle si quieres
    }

    void Chase()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Si está demasiado cerca, se detiene
        if (distance <= stopDistance)
            return;

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 flatDir = new Vector3(direction.x, 0, direction.z);

        rb.MovePosition(transform.position + flatDir * chaseSpeed * Time.deltaTime);

        if (flatDir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(flatDir);
    }

    void Attack()
    {
        // Solo mirar al jugador
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 flatDir = new Vector3(direction.x, 0, direction.z);

        if (flatDir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(flatDir);

        enemyAttack.TryAttack();

    }
}
