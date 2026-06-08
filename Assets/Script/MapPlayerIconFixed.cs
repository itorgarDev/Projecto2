using UnityEngine;
using UnityEngine.UI;

public class MapPlayerIconFixed : MonoBehaviour
{
    [Header("Referencias UI")]
    public RectTransform mapRect;        // RectTransform del panel del mapa (Image/RawImage)
    public RectTransform playerIcon;     // RectTransform del icono dentro del mapa (ideal: hijo directo)

    [Header("Jugador / Cámara")]
    public Transform playerWorld;        // Transform del jugador en la escena
    public Camera worldCamera;           // Cámara que renderiza el mundo (si null usa Camera.main)

    [Header("Opciones")]
    public Vector2 iconOffset = Vector2.zero;
    public bool roundToPixels = true;
    public bool debugLogs = true;

    Canvas parentCanvas;
    RenderMode canvasMode; // <-- tipo correcto

    void Start()
    {
        if (playerWorld == null || mapRect == null || playerIcon == null)
        {
            Debug.LogWarning("MapPlayerIcon_Fixed: faltan referencias (mapRect/playerIcon/playerWorld). Desactivando script.");
            enabled = false;
            return;
        }

        if (worldCamera == null) worldCamera = Camera.main;

        parentCanvas = mapRect.GetComponentInParent<Canvas>();
        if (parentCanvas != null) canvasMode = parentCanvas.renderMode;
        else canvasMode = RenderMode.ScreenSpaceOverlay;

        if (debugLogs)
        {
            Debug.Log($"MapPlayerIcon_Fixed iniciado. CanvasMode: {canvasMode}. worldCamera: {worldCamera}");
        }
    }

    void Update()
    {
        Vector3 playerPos = playerWorld.position;

        Camera camToUse = worldCamera != null ? worldCamera : Camera.main;
        Vector3 screenPoint = camToUse.WorldToScreenPoint(playerPos);

        if (screenPoint.z < 0f)
        {
            if (playerIcon.gameObject.activeSelf) playerIcon.gameObject.SetActive(false);
            if (debugLogs) Debug.Log("Jugador detrás de la cámara - icono oculto.");
            return;
        }
        else
        {
            if (!playerIcon.gameObject.activeSelf) playerIcon.gameObject.SetActive(true);
        }

        Vector2 localPoint;
        Camera rectCamera = (canvasMode == RenderMode.ScreenSpaceOverlay) ? null : camToUse;
        bool ok = RectTransformUtility.ScreenPointToLocalPointInRectangle(mapRect, screenPoint, rectCamera, out localPoint);

        if (!ok && debugLogs) Debug.LogWarning("ScreenPointToLocalPointInRectangle devolvió false.");

        Vector2 finalAnchored = localPoint + iconOffset;
        if (roundToPixels) finalAnchored = new Vector2(Mathf.Round(finalAnchored.x), Mathf.Round(finalAnchored.y));

        if (playerIcon.parent == mapRect)
        {
            playerIcon.anchoredPosition = finalAnchored;
        }
        else
        {
            Vector3 worldPos = mapRect.TransformPoint(localPoint);
            Vector3 parentLocal = playerIcon.parent.InverseTransformPoint(worldPos);
            Vector2 anchored = new Vector2(parentLocal.x, parentLocal.y) + iconOffset;
            if (roundToPixels) anchored = new Vector2(Mathf.Round(anchored.x), Mathf.Round(anchored.y));
            playerIcon.anchoredPosition = anchored;
        }

        if (debugLogs)
        {
            Debug.Log($"PlayerWorld: {playerPos} Screen: {screenPoint} LocalPoint: {localPoint} FinalAnchored: {playerIcon.anchoredPosition}");
        }
    }
}
