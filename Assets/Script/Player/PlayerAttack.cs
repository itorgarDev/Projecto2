using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStats stats;

    [SerializeField] private Collider weaponCollider; 
    [SerializeField] private float hitboxDuration = 0.3f;
    private bool hasDealtDamage = false;
    [SerializeField] private TrailRenderer weaponTrail;

    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        weaponCollider.enabled = false;
        stats = GetComponentInParent<PlayerStats>();

        if (weaponTrail != null)
        {
            weaponTrail.enabled = false;
        }
    }


    public void PerformAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("Attack");

        weaponCollider.enabled = true;

        if (weaponTrail != null)
        {
            weaponTrail.Clear(); // Limpia el trail de antes
            weaponTrail.enabled = true;
        }

        hasDealtDamage = false;
        Invoke(nameof(DisableCollider), hitboxDuration);
    }

    private void DisableCollider()
    {
        weaponCollider.enabled = false;

        if (weaponTrail != null)
        {
            weaponTrail.enabled = false;
        }
    }
    public void EndAttackAnimation() // cambia la variable en la animacion en el ultimo frame
    {
        isAttacking = false;
    }
    public void ForceCancelAttack() // si no llega al fin de la animacion se cancela (ej:dash muerte)
    {
        isAttacking = false;
        weaponCollider.enabled = false;

        if (weaponTrail != null)
        {
            weaponTrail.enabled = false;
        }

        // Esto asegura que el animator no se quede en un estado de bucle infinito
        animator.ResetTrigger("Attack");
        Debug.Log("[PlayerAttack] Ataque forzado a cancelar por dash");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hasDealtDamage) return;

        // Comprobamos si el objeto tiene la etiqueta de enemigo
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("OnTriggerEnter con objeto: " + other.name);

            // busca CUALQUIER script en el enemigo que use el sistema de daño
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                // Si encontramo el sistema, le mandamos el daño de nuestros stats
                float damageToDeal = stats != null ? stats.attack : 1f;

                damageable.SystemTakeDamage(damageToDeal);
                hasDealtDamage = true;
            }
            else
            {
                // Por si acaso no encuentra la interfaz del daño
                Debug.LogWarning($"El objeto {other.name} es 'Enemy' pero no tiene configurada la interfaz IDamageable.");
            }
        }
    }
}

