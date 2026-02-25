using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private Collider weaponCollider;
    [SerializeField] private float hitboxDuration = 0.3f;
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private int damage = 1;

    private float lastAttackTime = -Mathf.Infinity;

    private void Awake()
    {
        if (weaponCollider != null)
            weaponCollider.enabled = false;
    }

    public void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;
        PerformAttack();
    }

    private void PerformAttack()
    {
        // Aquí puedes poner animación más adelante
        weaponCollider.enabled = true;
        Invoke(nameof(DisableCollider), hitboxDuration);
    }

    private void DisableCollider()
    {
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}
