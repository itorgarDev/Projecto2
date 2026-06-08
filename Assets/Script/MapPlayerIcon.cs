using UnityEngine;
using UnityEngine.UI;

public class MapPlayerIcon : MonoBehaviour
{
    public Canvas mapCanvas;               // Canvas que contiene el mapa
    public RectTransform mapRect;          // RectTransform del panel del mapa (imagen del mapa)
    public RectTransform playerIcon;       // RectTransform del icono dentro del mapa
    public Transform playerWorld;          // Transform del jugador en el mundo
    public Vector2 worldMin;               // Esquina mínima del mundo que cubre el mapa (x,z)
    public Vector2 worldMax;               // Esquina máxima del mundo que cubre el mapa (x,z)
    public bool rotateWithPlayer = true;   // Si el icono debe rotar según la orientación del jugador
    public float iconRotationOffset = 0f;

    void Update()
    {
        if (playerWorld == null || playerIcon == null || mapRect == null) return;

        // Normalizar posición del jugador dentro del rectángulo del mundo
        Vector2 worldPos2D = new Vector2(playerWorld.position.x, playerWorld.position.z);
        Vector2 normalized = new Vector2(
            Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos2D.x),
            Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos2D.y)
        );

        // Convertir normalizado a posición local dentro del RectTransform del mapa
        Vector2 mapSize = mapRect.rect.size;
        Vector2 localPos = new Vector2(
            (normalized.x - 0.5f) * mapSize.x,
            (normalized.y - 0.5f) * mapSize.y
        );

        // Debug.Log aquí
        Debug.Log($"Player world: {worldPos2D} Normalized: {normalized} LocalPos: {localPos} MapSize: {mapSize}");

        playerIcon.anchoredPosition = localPos;

        if (rotateWithPlayer)
        {
            float yRotation = playerWorld.eulerAngles.y + iconRotationOffset;
            playerIcon.localEulerAngles = new Vector3(0, 0, -yRotation);
        }

    }
}
