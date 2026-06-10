using UnityEngine;

public class DummyDamageable : MonoBehaviour, IDamageable
{
    [Header("Configuración de Vida")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public GameObject key;

    
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
        SoundController.Instance.PlaySFX(SoundController.Instance.wood);
        Debug.Log($"{gameObject.name} recibió {amount} de daño. Vida restante: {currentHealth}");

        if (showBarInHUD && HUDController.Instance != null && currentHealth > 0)
        {
            
            HUDController.Instance.TrackDummy(this, objectName, maxHealth);

            
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
       
        if (HUDController.Instance != null)
        {
            HUDController.Instance.UntrackDummy();
        }
    }

    private void Die()
    {
        SoundController.Instance.PlaySFX(SoundController.Instance.zExplosion);
        if (HUDController.Instance != null)
        {
            HUDController.Instance.UntrackDummy();
        }
        if (key != null)
        {
            key.SetActive(true);
        }
        Destroy(gameObject);
    }
}