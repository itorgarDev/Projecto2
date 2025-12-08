using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllayMovement : MonoBehaviour
{
    [SerializeField] private Transform player;         // Referencia al jugador
    [SerializeField] private float followSpeed = 3f;   // Velocidad normal de seguimiento
    [SerializeField] private float sprintSpeed = 6f;   // Velocidad cuando se queda atrás
    [SerializeField] private float acceleration = 10f; // Qué tan rápido cambia de velocidad
    [SerializeField] private float minDistance = 1.5f; // Distancia mínima para seguir
    [SerializeField] private float maxDistance = 6f;   // Distancia máxima antes de acelerar

    private float rotationOffsetZ = 90f; // CORRECCIÓN EN Z

    private float currentSpeed = 0f;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > minDistance)
        {
            float targetSpeed = (distance > maxDistance) ? sprintSpeed : followSpeed;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * currentSpeed * Time.deltaTime;

            // Mantener LookAt pero corrigiendo en Z
            transform.LookAt(player);
            transform.rotation *= Quaternion.Euler(0f, 0f, rotationOffsetZ);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
        }
    }
}
