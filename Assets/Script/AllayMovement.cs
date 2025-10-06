using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllayMovement : MonoBehaviour
{
    [SerializeField] private Transform player;      // Referencia al jugador
    [SerializeField] private float followSpeed = 3f; // Velocidad de seguimiento
    [SerializeField] private float followDelay = 0.1f; // Suavizado del movimiento
    [SerializeField] private float minDistance = 1.5f; // Distancia mínima para seguir

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position); // hace que no se choque con el jugador

        // Calcula la posición suavizada
        Vector3 targetPosition = player.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followDelay, followSpeed);
        transform.LookAt(player);// mira al jugador
    }
}
