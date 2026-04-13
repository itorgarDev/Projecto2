using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Animator animatorWeapon;
    [Header("Ataque")]
    [SerializeField] private Collider weaponCollider;
    [SerializeField] private float hitboxDuration = 0.3f;
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private int damage = 1;

    private float lastAttackTime = -Mathf.Infinity;

    private void Awake()
    {
        animatorWeapon = GetComponentInChildren<Animator>();
        weaponCollider.enabled = false;
    }

    public void TryAttack()
    {
        Debug.Log("EnemyAttack: TryAttack() llamado");
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;
        PerformAttack();
    }

    private void PerformAttack()
    {
        animatorWeapon.SetTrigger("Attack");

        weaponCollider.enabled = true;
        Invoke(nameof(DisableCollider), hitboxDuration);
    }


    private void DisableCollider()
    {
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hitbox tocó: " + other.name);
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
       
    }
}
