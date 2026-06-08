using UnityEngine;
using UnityEngine.UI;

public class MapPlayerIcon : MonoBehaviour
{
    public Canvas mapCanvas;               // Canvas que contiene el mapa (UI)
    public RectTransform mapRect;          // RectTransform de la imagen del pergamino (UI)
    public RectTransform playerIcon;       // RectTransform del icono del panda (UI)
    public Transform playerWorld;          // Transform del jugador en el Escenario 3D
    public Transform cornerMinObject;      // Esquina mínima (Sur-Oeste) en el Escenario 3D
    public Transform cornerMaxObject;      // Esquina máxima (Norte-Este) en el Escenario 3D
    public bool rotateWithPlayer = true;   // Si el icono debe rotar según la orientación del jugador
    public float iconRotationOffset = 0f;

    void Update()
    {
        // Seguridad: Si falta algo por arrastrar en el Inspector, no hacemos nada para evitar errores
        if (playerWorld == null || playerIcon == null || mapRect == null || cornerMinObject == null || cornerMaxObject == null) return;

        // 1. PASO DEL ESCENARIO 3D A VECTOR2:
        // Guardamos la X del mundo en la 'x' de nuestro Vector2.
        // Guardamos la Z del mundo (profundidad) en la 'y' de nuestro Vector2.
        Vector2 worldMin = new Vector2(cornerMinObject.position.x, cornerMinObject.position.z);
        Vector2 worldMax = new Vector2(cornerMaxObject.position.x, cornerMaxObject.position.z);
        Vector2 worldPos2D = new Vector2(playerWorld.position.x, playerWorld.position.z);

        // 2. NORMALIZACIÓN MATEMÁTICA:
        // Calculamos el porcentaje (de 0 a 1) de dónde está el jugador respecto a los corners.
        // CORREGIDO: Ahora cada eje usa estrictamente sus componentes correctas (x con x, y con y).
        Vector2 normalized = new Vector2(
            Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos2D.x),
            Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos2D.y)
        );

        // 3. PASO A LA IMAGEN 2D (UI):
        // Convertimos ese porcentaje en píxeles dentro del tamaño actual del mapa en la pantalla.
        // Restamos 0.5f porque el pivote de tu mapa está en el centro.
        Vector2 mapSize = mapRect.rect.size;
        Vector2 localPos = new Vector2(
            (normalized.x - 0.5f) * mapSize.x,
            (normalized.y - 0.5f) * mapSize.y
        );

        // Debug.Log para comprobar que los datos no se crucen en la consola
        Debug.Log($"MUNDO 3D: {worldPos2D} | NORMALIZADO: {normalized} | UI MAPA: {localPos} | TAMAÑO MAPA: {mapSize}");

        // 4. APLICAR POSICIÓN:
        // CORREGIDO: Usamos 'anchoredPosition3D' forzando la Z a 0f. 
        // Esto evita que el icono del panda se hunda por detrás de la imagen del pergamino al moverte.
        playerIcon.anchoredPosition3D = new Vector3(localPos.x, localPos.y, 0f);

        // 5. ROTACIÓN:
        if (rotateWithPlayer)
        {
            float yRotation = playerWorld.eulerAngles.y + iconRotationOffset;
            playerIcon.localEulerAngles = new Vector3(0, 0, -yRotation);
        }
    }
}