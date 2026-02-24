using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum AgentStates { idle, attack }
    public AgentStates currentState;

    public float speed = 2f;

    [Header("Vision")]
    public float visionRange = 8f;
    public float visionAngle = 45f;
    public LayerMask obstacleMask;

    Rigidbody rb;
    Transform player; // para localizar al jugador

    public Transform model;

    private void Start()
    {
        currentState = AgentStates.idle;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        currentState = ChangeState();

        switch (currentState)
        {
            case AgentStates.idle:
                Idle();
                break;

            case AgentStates.attack:
                Attack();
                break;
        }
    }

    AgentStates ChangeState()
    {
        if (CanSeePlayer())
        {
            return AgentStates.attack;
        }

        return AgentStates.idle;
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Comprobar ángulo de visión
        if (Vector3.Angle(transform.forward, directionToPlayer) < visionAngle)
        {
            // Comprobar distancia
            if (distanceToPlayer < visionRange)
            {
                // Raycast para ver si hay obstáculos
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
        // Añadir animacion de enemigo quieto
    }

    void Attack()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // Movimiento 
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

        // Rotación SOLO en el plano horizontal para que no se incline
        Vector3 flatDir = new Vector3(direction.x, 0, direction.z);

        if (flatDir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(flatDir);
    }

}
