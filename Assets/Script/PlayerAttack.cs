using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    
    [SerializeField] private Collider weaponCollider; // arrastra WeaponHitbox aquí
    [SerializeField] private float hitboxDuration = 0.3f;
    [SerializeField] private int damage = 50;
    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    void Awake()
    {
        
        weaponCollider.enabled = false;
    }

    public void PerformAttack()
    {
        isAttacking = true;
        weaponCollider.enabled = true;
        Invoke(nameof(DisableCollider), hitboxDuration);
    }

    private void DisableCollider()
    {
        weaponCollider.enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter con objeto: " + other.name);
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            Debug.Log("Enemy detectado, aplicando daño: " + damage);
            enemy.TakeDamage(damage);
        }
    }
}
