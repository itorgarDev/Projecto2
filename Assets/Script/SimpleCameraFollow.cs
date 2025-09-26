using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private float smoothTime;
    public Vector3 offset;
    private Vector3 _currentVelocity = Vector3.zero;
    [SerializeField] private float lookAheadDistance = 2f; // Distancia de anticipación al movimiento del objetivo

    private Vector3 lastTargetPosition;

    private void Start()
    {
        lastTargetPosition = target.position;
    }

    private void LateUpdate()
    {
        // Calcula la dirección de movimiento solo en el plano XZ
        Vector3 moveDelta = target.position - lastTargetPosition;
        Vector3 moveDirection = new Vector3(moveDelta.x, 0, moveDelta.z).normalized;
        lastTargetPosition = target.position;

        // Offset dinámico solo en XZ
        Vector3 dynamicOffset = offset + moveDirection * lookAheadDistance;

        Vector3 targetPosition = target.position + dynamicOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime); // Suaviza el movimiento de la cámara
    }
}