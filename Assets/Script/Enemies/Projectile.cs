using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
    public enum ShootPattern { SingleBullet, Burst }

    [Header("Pattern Setup")]
    public ShootPattern shootPattern;

    [Header("Stats and Damage")]
    public float damage = 1f;
    public float speed = 24f;
    public float lifeTime = 15f;

    [Header("Tracking Setup")]
    private Transform target;
    public float trackingStrength = 3f; // cuanto mas alto mas gira la bala 

    [Header("Burst Setup")]
    public int burstAmount = 3;
    public float timeBetweenBullets = 1f;

    [HideInInspector] public bool isClone = false;
    private Coroutine burstCoroutine;

    private void OnEnable()
    {
        CancelInvoke(nameof(DeactivateAndReturnToPool));
        Invoke(nameof(DeactivateAndReturnToPool), lifeTime);

        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("[Bala] no encuentro a nadie con el tag 'Player' en la escena!");
            }
        }

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
        // Si hay un jugador, rotamos hacia el poco a poco
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, trackingStrength * Time.deltaTime);
            }

            // Seguro para que no de vueltas en circulos infinitos si te esquiva de cerca
            if (Vector3.Distance(transform.position, target.position) < 4f)
            {
                target = null;
            }
        }

        
        float moveDistance = speed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, moveDistance + 0.2f))
        {
            // Si el raycast choca, procesamos el impacto y no nos movemos mas
            ProcessingHit(hit.collider);
            return;
        }

        // Si el camino esta limpio, avanza
        transform.Translate(Vector3.forward * moveDistance);
    }
    private void ProcessingHit(Collider other)
    {
        // Ignoramos si choca con otra bala 
        if (other.GetComponent<Projectile>() != null)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        // DAÑO AL JUGADOR
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Debug.Log("[Raycast] Impacto al jugador.");
            player.TakeDamage((int)damage);
            DeactivateAndReturnToPool();
            return;
        }

        // DAÑO A OTRAS COSAS DAÑABLES
        IDamageable victim = other.GetComponent<IDamageable>();
        if (victim != null)
        {
            victim.SystemTakeDamage(damage);
            DeactivateAndReturnToPool();
            return;
        }

        // SI CHOCA CON TODO LO DEMAS
        Debug.Log("[Raycast] Choco contra: " + other.name);
        DeactivateAndReturnToPool();
    }
    private void DeactivateAndReturnToPool()
    {
        isClone = false;
        ProjectilePool.Instance.ReturnToPool(gameObject);
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
            // El i*-1.5f desplaza la bala hacia atras cada vez
            Vector3 spawnOffset = transform.forward * (i * -1.5f);
            GameObject clone = ProjectilePool.Instance.GetProjectile(transform.position + spawnOffset, transform.rotation);

            if (clone != null)
            {
                Projectile pScript = clone.GetComponent<Projectile>();
                if (pScript != null) pScript.isClone = true;
                clone.SetActive(true);
            }
            yield return new WaitForSeconds(timeBetweenBullets);
        }
        DeactivateAndReturnToPool();
    }
   
}