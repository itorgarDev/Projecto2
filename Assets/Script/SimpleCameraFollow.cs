using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleCameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private float smoothTime;
    public Vector3 offset;
    private Vector3 _currentVelocity = Vector3.zero;
    [SerializeField] private float lookAheadDistance = 2f; // Distancia de anticipación al movimiento del objetivo

    private Vector3 lastTargetPosition;

    private static SimpleCameraFollow instance;


    private void Awake()
    { /*
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 2. Si estamos en la escena 0 → destruir Player
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Destroy(gameObject);
            return;
        }

        // 3. Registrar este Player como el único
        //instance = this;

        // 4. Hacerlo persistente SOLO si no estamos en escena 0
        DontDestroyOnLoad(gameObject);
        */
    }

    private void Start()
    {
        lastTargetPosition = target.position;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
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