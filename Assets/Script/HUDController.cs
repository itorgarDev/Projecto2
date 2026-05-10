using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Importante para usar 'Image'

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [Header("Player Stats")]
    public PlayerStats stats;

    [Header("Componentes de la Barra")]
    // Cambiamos RectTransform por Image para usar el sistema Filled
    public Image backgroundBar; // lowHP.png
    public Image healthFill;    // FullHp.png

    [Header("Ajustes de Crecimiento")]
    public float baseHealth = 5f;       // La vida inicial del jugador (para no crecer la barra si no pasas de este nivel)
    public float baseWidth = 400f;       // El tamaño inicial de tus imágenes
    public float pixelsPerExtraHealth = 10f; // Cuántos píxeles crece por CADA punto extra de vida por encima de la base

    
    [Header("Pickup Message")]
    public GameObject pickupPanel;
    public TMP_Text pickupText;
    private Coroutine pickupCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (stats.maxHealth <= 0) return; // Seguridad

        // 1. Calculamos cuánta vida EXTRA tiene el jugador respecto a la base
        // Mathf.Max evita números negativos si la vida máxima baja por alguna maldición o algo
        float extraHealth = Mathf.Max(0, stats.maxHealth - baseHealth);

        // 2. Calculamos el ancho total: El base (400) + los píxeles extra
        float currentWidth = baseWidth + (extraHealth * pixelsPerExtraHealth);

        // 3. Estiramos el contenedor (Fondo) y el relleno para que midan lo mismo
        backgroundBar.rectTransform.sizeDelta = new Vector2(currentWidth, backgroundBar.rectTransform.sizeDelta.y);
        healthFill.rectTransform.sizeDelta = new Vector2(currentWidth, healthFill.rectTransform.sizeDelta.y);

        // 4. LA CLAVE: Actualizamos el color usando FillAmount de 0 a 1. 
        // ¡Adiós a los huecos negros y a las matemáticas de desplazamiento!
        healthFill.fillAmount = (float)stats.currentHealth / stats.maxHealth;
    }

    // ===========================
    //   SISTEMA DE PICKUP
    // ===========================
    public void ShowPickupMessage(string itemName)
    {
        pickupText.text = "Has recogido: " + itemName;
        pickupPanel.SetActive(true);

        // Usamos la variable para no detener otras posibles corrutinas del HUD
        if (pickupCoroutine != null) StopCoroutine(pickupCoroutine);
        pickupCoroutine = StartCoroutine(HidePickupRoutine());
    }

    private IEnumerator HidePickupRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        pickupPanel.SetActive(false);
    }
}