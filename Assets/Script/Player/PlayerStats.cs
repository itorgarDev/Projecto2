using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public HUDController hud;

    [Header("Vida")]
    public int maxHealth = 3;
    public int currentHealth = 3;

    [Header("Ataque")]
    public int attack = 1;

    void Start()
    {
        LoadStats();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        hud.UpdateHealthBar();
    }

    public void AddAttack(int amount)
    {
        attack += amount;
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; // se cura al nuevo máximo
        hud.UpdateHealthBar();

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;

        hud.UpdateHealthBar();
    }

    public void SaveStats()
    {
        PlayerPrefs.SetInt("player_maxHealth", maxHealth);
        PlayerPrefs.SetInt("player_currentHealth", currentHealth);
        PlayerPrefs.SetInt("player_attack", attack);

        PlayerPrefs.Save();
    }

    public void LoadStats()
    {
        // Si no existe el dato, usa el valor actual como predeterminado
        maxHealth = PlayerPrefs.GetInt("player_maxHealth", maxHealth);
        currentHealth = PlayerPrefs.GetInt("player_currentHealth", currentHealth);
        attack = PlayerPrefs.GetInt("player_attack", attack);

        hud.UpdateHealthBar();
    }

    void Update()
    {
        // Solo para testeo: Si pulsas R, borras los datos y reinicias la escena
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Datos borrados. Reinicia el juego para ver los cambios.");
        }
    }
}
