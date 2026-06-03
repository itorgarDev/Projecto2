using UnityEngine;

public class DummyDamageable : MonoBehaviour, IDamageable
{
    [Header("Configuración de Vida")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    // --- ESTO ES OBLIGATORIO: Permite al HUDController leer la vida en el Update ---
    public float CurrentHealth => currentHealth;

    [Header("Configuración del HUD")]
    [SerializeField] private bool showBarInHUD = true;
    [SerializeField] private string objectName = "Estatua Dragón";

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void SystemTakeDamage(float amount)
    {
        currentHealth -= amount;

        Debug.Log($"{gameObject.name} recibió {amount} de daño. Vida restante: {currentHealth}");

        if (showBarInHUD && HUDController.Instance != null && currentHealth > 0)
        {
            // --- REVISA ESTA LÍNEA: Debe llamar a TrackDummy pasándole (this, nombre, vidaMáxima) ---
            HUDController.Instance.TrackDummy(this, objectName, maxHealth);

            // Temporizador para ocultar la barra si el jugador se aleja o no le pega más
            CancelInvoke(nameof(HideBarDueToInactivity));
            Invoke(nameof(HideBarDueToInactivity), 4f);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void HideBarDueToInactivity()
    {
        // --- REVISA ESTA LÍNEA: Debe llamar a UntrackDummy ---
        if (HUDController.Instance != null)
        {
            HUDController.Instance.UntrackDummy();
        }
    }

    private void Die()
    {
        // --- REVISA ESTA LÍNEA: Debe llamar a UntrackDummy ---
        if (HUDController.Instance != null)
        {
            HUDController.Instance.UntrackDummy();
        }

        Destroy(gameObject);
    }
}