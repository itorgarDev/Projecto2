using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private MeshCollider weaponCollider;
    [SerializeField] private int damage = 50;

    void Awake()
    {
        anim = GetComponent<Animator>();
        weaponCollider = GetComponent<MeshCollider>(); // si usas MeshCollider
        weaponCollider.convex = true; // obligatorio para triggers
        weaponCollider.isTrigger = true;
        weaponCollider.enabled = false;
    }

    // Este método lo llamará PlayerMovement cuando detecte el input "Attack"
    public void PerformAttack()
    {
        anim.SetTrigger("Attack");
        weaponCollider.enabled = true; // activa el collider al empezar la animación
        Invoke(nameof(DisableCollider), anim.GetCurrentAnimatorStateInfo(0).length);
        // desactiva el collider automáticamente cuando termine el estado actual
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
        else
        {
            Debug.Log("El objeto no tiene componente Enemy");
        }
    }
}

