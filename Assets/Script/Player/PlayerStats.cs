using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 3;
    public int currentHealth = 3;

    [Header("Ataque")]
    public int attack = 1;

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public void AddAttack(int amount)
    {
        attack += amount;
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; // se cura al nuevo máximo
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;
    }
}
