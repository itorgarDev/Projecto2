using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum ShootPattern { SingleBullet, Burst }

    [Header("Pattern Setup")]
    public ShootPattern shootPattern;

    [Header("Stats and Damage")]
    public float damage = 1f;
    public float speed = 18f;
    public float lifeTime = 15f;

    [Header("Burst Setup")]
    public int burstAmount = 3;
    public float timeBetweenBullets = 0.2f;

    [HideInInspector] public bool isClone = false;
    private Coroutine burstCoroutine;

    private void OnEnable()
    {
        CancelInvoke(nameof(DeactivateAndReturnToPool));
        Invoke(nameof(DeactivateAndReturnToPool), lifeTime);

        if (!isClone)
        {
            ExecutePattern();
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
        if (burstCoroutine != null)
        {
            StopCoroutine(burstCoroutine);
            burstCoroutine = null;
        }
    }

    private void Update()
    {
        // Todas las balas avanzan siempre
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void ExecutePattern()
    {
        switch (shootPattern)
        {
            case ShootPattern.SingleBullet:
                break;

            case ShootPattern.Burst:
                burstCoroutine = StartCoroutine(CoroutineBurst());
                break;
        }
    }

    private IEnumerator CoroutineBurst()
    {
        for (int i = 0; i < burstAmount; i++)
        {
            GameObject clone = ProjectilePool.Instance.GetProjectile(transform.position, transform.rotation);

            if (clone != null)
            {
                Projectile pScript = clone.GetComponent<Projectile>();
                if (pScript != null)
                {
                    pScript.isClone = true;
                }
                clone.SetActive(true);
            }
            yield return new WaitForSeconds(timeBetweenBullets);
        }

        DeactivateAndReturnToPool();
    }

    // --- SISTEMA DE DAÑO LIBERADO Y DIRECTO ---
    private void OnTriggerEnter(Collider other)
    {
        // 1. DAÑO AL JUGADOR
        // Recuerda: Si el script de tu jugador se llama diferente a 'PlayerMovement', cambia ese nombre aquí abajo
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Debug.Log("[Bala] ¡Golpe directo al jugador! Quitando vida: " + damage);
            player.TakeDamage((int)damage);
            DeactivateAndReturnToPool();
            return;
        }

        // 2. DAÑO A ENEMIGOS (Por si acaso)
        IDamageable victim = other.GetComponent<IDamageable>();
        if (victim != null)
        {
            victim.SystemTakeDamage(damage);
            DeactivateAndReturnToPool();
            return;
        }

        // 3. IMPACTO CONTRA ESCENARIO (Muros o Suelo)
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            DeactivateAndReturnToPool();
        }
    }

    private void DeactivateAndReturnToPool()
    {
        isClone = false;
        ProjectilePool.Instance.ReturnToPool(gameObject);
    }
}