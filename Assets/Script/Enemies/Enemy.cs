using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2;
    private int currentHealth;
    [SerializeField] bool isBoss = false;
    public bool IsBoss => isBoss;

    public int CurrentHealth => currentHealth;

    // Se ejecuta cada vez que el enemigo se activa (cuando sale del pool)
    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Enemy recibió daño. Vida restante: " + currentHealth);
        if (isBoss)
        {
            var boss = GetComponent<BossEvokerFSMManager>();
            if (boss != null)
            {
                Debug.Log("[BOSS VIDA] "+ CurrentHealth);
                Debug.Log("[Enemy] Soy un boss, llamando a EvaluateWaves()");
                boss.EvaluateWaves();
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public event System.Action OnDeath; // evento que avisa cuando muere un enemigo
    private void Die()
    {
        Debug.Log("Enemy muerto");
        OnDeath?.Invoke();
        EnemyPool.Instance.ReturnToPool(this);
    }
}
