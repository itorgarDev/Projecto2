using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStats stats;

    [SerializeField] private Collider weaponCollider; // arrastra WeaponHitbox aquí
    [SerializeField] private float hitboxDuration = 0.3f;
    private bool hasDealtDamage = false;

    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    void Awake()
    {
        
        weaponCollider.enabled = false;
        stats = GetComponentInParent<PlayerStats>();

    }

    public void PerformAttack()
    {
        isAttacking = true;
        weaponCollider.enabled = true;
        hasDealtDamage = false;
        Invoke(nameof(DisableCollider), hitboxDuration);
    }

    private void DisableCollider()
    {
        weaponCollider.enabled = false;
        isAttacking = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasDealtDamage) return;
        Debug.Log("OnTriggerEnter con objeto: " + other.name);
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (enemy.IsBoss)
            {
                var boss = enemy.GetComponent<BossEvokerFSMManager>();
                if (boss != null && boss.isShielded)
                {
                    Debug.Log("[PlayerAttack] Boss tiene escudo, NO hago daño");
                    return; // NO HACEMOS DAÑO, NO ACUMULAMOS NADA
                }
            }


            enemy.TakeDamage(stats.attack);
            hasDealtDamage = true;
        }
    }
}
