using Unity.VisualScripting;
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

    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        weaponCollider.enabled = false;
        stats = GetComponentInParent<PlayerStats>();
    }


    public void PerformAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("Attack");

        weaponCollider.enabled = true;
        hasDealtDamage = false;
        Invoke(nameof(DisableCollider), hitboxDuration);
    }

    private void DisableCollider()
    {
        weaponCollider.enabled = false;    
    }
    public void EndAttackAnimation()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasDealtDamage) return;
        
        
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("OnTriggerEnter con objeto: " + other.name);
            if (other.gameObject.GetComponent<EnemyFSMManager>().IsBoss)
            {
                var boss = other.gameObject.GetComponent<BossEvokerFSMManager>();
                if (boss != null && boss.isShielded)
                {
                    Debug.Log("[PlayerAttack] Boss tiene escudo, NO hago daño");
                    return; // NO HACEMOS DAÑO, NO ACUMULAMOS NADA
                }
            }


            other.gameObject.GetComponent<EnemyFSMManager>().TakeDamage(stats.attack);
            hasDealtDamage = true;
        }
    }
}
