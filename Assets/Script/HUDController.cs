using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [Header("Player Stats")]
    public PlayerStats stats;

    [Header("Health Bar")]
    public RectTransform healthMask;
    public RectTransform healthFill;

    [Header("Health Bar Settings")]
    public float pixelsPerHealth = 30f; // Cuánto crece la barra por cada punto de vida
    public float baseWidth = 300f;

    // ===========================
    //      MENSAJES DE PICKUP
    // ===========================
    [Header("Pickup Message")]
    public GameObject pickupPanel;   // Panel donde aparece el mensaje
    public TMP_Text pickupText;      // Texto del mensaje

    private void Awake()
    {
        Instance = this; // Para poder llamar HUDController.Instance desde otros scripts
    }

    void Start()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        // decide el ancho de la barra total
        float totalWidth = baseWidth + (stats.maxHealth * pixelsPerHealth);

        // agrandamos la barra al subir vida max
        healthMask.sizeDelta = new Vector2(totalWidth, healthMask.sizeDelta.y);

        // calculamos el porcentaje de vida
        float percent = (float)stats.currentHealth / stats.maxHealth;
        percent = Mathf.Clamp01(percent);

        // ajustamos el color rojo en funcion del porcentaje
        healthFill.sizeDelta = new Vector2(totalWidth * percent, healthFill.sizeDelta.y);
    }

    // ===========================
    //      FUTURAS BARRAS
    // ===========================
    public void UpdateStaminaBar(float percent)
    {
        // Placeholder
    }

    public void UpdateManaBar(float percent)
    {
        // Placeholder
    }

    // ===========================
    //   SISTEMA DE PICKUP
    // ===========================
    public void ShowPickupMessage(string itemName)
    {
        pickupText.text = "Has recogido: " + itemName;
        pickupPanel.SetActive(true);

        StopAllCoroutines(); // Evita que se solapen mensajes
        StartCoroutine(HidePickupRoutine());
    }

    private IEnumerator HidePickupRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        pickupPanel.SetActive(false);
    }
}
