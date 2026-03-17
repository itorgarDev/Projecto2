using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Collider weaponCollider; // arrastra WeaponHitbox aquí
    [SerializeField] private float hitboxDuration = 0.3f;
    [SerializeField] private int damage = 50;

    void Awake()
    {
        anim = GetComponent<Animator>();
        weaponCollider.enabled = false;
    }

    public void PerformAttack()
    {
        anim.SetTrigger("Attack");
        weaponCollider.enabled = true;
        Invoke(nameof(DisableCollider), hitboxDuration);
    }

    private void DisableCollider()
    {
        weaponCollider.enabled = false;
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
