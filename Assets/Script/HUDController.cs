using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Importante para usar 'Image'

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [Header("Player Stats")]
    public PlayerStats stats;

    [Header("Componentes de la Barra de Vida")]
    public Image backgroundBar; // lowHP.png
    public Image healthFill;    // FullHp.png

    [Header("Ajustes de Crecimiento de Vida")]
    public float baseHealth = 5f;       // La vida inicial del jugador (para no crecer la barra si no pasa de este nivel)
    public float baseWidth = 400f;      
    public float pixelsPerExtraHealth = 10f; // Cuántos píxeles crece por CADA punto extra de vida por encima de la base

    [Header("Componentes del Dash")]
    public Image dashFillImage; 
    public GameObject dashGlow; 

    [Header("Pickup Message")]
    public GameObject pickupPanel;
    public TMP_Text pickupText;
    private Coroutine pickupCoroutine;

    [Header("Componentes de Jefes")]
    public GameObject bossPanel;       // bossUI
    public Image bossHealthFill; // barra llena
    public GameObject bossNamePanel; // marco del nombre
    public TMP_Text bossNameText;      // Texto para mostrar el nombre del Boss actual
    public GameObject dummyPanel;       // bossUI
    public Image dummyHealthFill;
    public GameObject dummyNamePanel; 
    public TMP_Text dummyNameText;

    private PhoenixFSM activeFenix;
    private EnemyFSMManager activeEvoker;
    private DummyDamageable activeDummy; 
    private float activeBossMaxHp;
    private float activeDummyMaxHp;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateHealthBar();
        if (bossPanel != null) bossPanel.SetActive(false);
        if (dummyPanel != null) dummyPanel.SetActive(false);
    }
    void Update()
    {
        if (bossPanel != null && bossPanel.activeSelf)
        {
            if (activeFenix != null)
            {
                // Calcula el porcentaje de vida y actualiza el relleno
                bossHealthFill.fillAmount = Mathf.Clamp01(activeFenix.Health / activeBossMaxHp);
                if (activeFenix.isDead) UntrackBoss();
            }
            else if (activeEvoker != null)
            {
                // Calcula el porcentaje de vida y actualiza el relleno
                bossHealthFill.fillAmount = Mathf.Clamp01(activeEvoker.CurrentHealth / activeBossMaxHp);
                if (activeEvoker.CurrentHealth <= 0) UntrackBoss();
            }
        }
        if (dummyPanel != null && dummyPanel.activeSelf)
        {
            if (activeDummy != null)
            {
                dummyHealthFill.fillAmount = Mathf.Clamp01(activeDummy.CurrentHealth / activeDummyMaxHp);
                if (activeDummy.CurrentHealth <= 0) UntrackDummy();
            }
        }
    }
    public void UpdateHealthBar()
    {
        if (stats.maxHealth <= 0) return; // Seguridad

        // Calculamos cuánta vida EXTRA tiene el jugador respecto a la base
        // Mathf.Max evita números negativos por si acaso
        float extraHealth = Mathf.Max(0, stats.maxHealth - baseHealth);

        // Calculamos el ancho total: El base (400) + los píxeles extra
        float currentWidth = baseWidth + (extraHealth * pixelsPerExtraHealth);

        // Estiramos el contenedor (Fondo) y el relleno para que midan lo mismo
        backgroundBar.rectTransform.sizeDelta = new Vector2(currentWidth, backgroundBar.rectTransform.sizeDelta.y);
        healthFill.rectTransform.sizeDelta = new Vector2(currentWidth, healthFill.rectTransform.sizeDelta.y);

        //Actualizamos el color usando FillAmount. 
        healthFill.fillAmount = (float)stats.currentHealth / stats.maxHealth;
    }

    public void UpdateDashCooldown(float timeElapsed, float totalCooldown)
    {
        if (totalCooldown <= 0) return;

        // Calculamos el progreso (0 a 1)
        float progress = Mathf.Clamp01(timeElapsed / totalCooldown);

        // La ola se llena verticalmente
        dashFillImage.fillAmount = progress;

        // Si el progreso es 1 (listo), activamos el marco. Si no, lo apagamos.
        if (progress >= 1f)
        {
            if (!dashGlow.activeSelf) dashGlow.SetActive(true);
        }
        else
        {
            if (dashGlow.activeSelf) dashGlow.SetActive(false);
        }
    }
    // utilizamos track ya sea boss o dummy para activar los paneles segun nos convengan
    public void TrackBoss(PhoenixFSM fenix, EnemyFSMManager evoker, string bossName, float maxHp)
    {
        activeFenix = fenix;
        activeEvoker = evoker;
        activeBossMaxHp = maxHp;

        if (bossPanel != null) bossPanel.SetActive(true);
        if (bossNamePanel != null) bossNamePanel.SetActive(true);
        if (bossNameText != null) bossNameText.text = bossName;
    }
    public void TrackDummy(DummyDamageable dummy, string displayName, float maxHp)
    {
        activeDummy = dummy;
        activeDummyMaxHp = maxHp; 

        if (dummyPanel != null) dummyPanel.SetActive(true);
        if (dummyNamePanel != null) dummyNamePanel.SetActive(true);
        if (dummyNameText != null) dummyNameText.text = displayName;
    }
    // con untrack dejamos de mostralo en caso de alejarnos o matar al boss
    public void UntrackBoss()
    {
        activeFenix = null;
        activeEvoker = null;
        activeDummy = null;
        if (bossPanel != null) bossPanel.SetActive(false);
    }
    public void UntrackDummy()
    {
        activeDummy = null;
        activeDummyMaxHp = 0;
        if (dummyPanel != null) dummyPanel.SetActive(false); // Apaga el panel del dummy
    }
    public void ShowPickupMessage(string itemName)
    {
        pickupText.text = "Has recogido: " + itemName;
        pickupPanel.SetActive(true);

        
        if (pickupCoroutine != null) StopCoroutine(pickupCoroutine);
        pickupCoroutine = StartCoroutine(HidePickupRoutine());
    }

    private IEnumerator HidePickupRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        pickupPanel.SetActive(false);
    }
}