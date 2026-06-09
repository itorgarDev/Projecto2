using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvokerFSMManager : EnemyFSMManager
{
    private EnemyFSMManager enemy;

    //estados
    public BossIdle bossIdleState;
    public BossAttack bossAttackState;
    public Summon summonState;
    public Shield shieldState;

    //variables
    public int currentWave = 0;
    public int aliveMinions = 0;
    public bool isShielded = false;

    public Animator animatorBoss;


    protected override void Awake() 
    {
        base.Awake();

        // Volvemos a forzar sus stats de Boss para que no herede los de un enemigo comun
        InitializeStats();

        animator = animatorBoss;
        enemy = GetComponent<EnemyFSMManager>();

        // Creamos la instancia específica del boss y la asignamos también al campo base `idleState`
        bossIdleState = new BossIdle(this);
        idleState = bossIdleState;

        bossAttackState = new BossAttack(this);

        shieldState = new Shield(this);
        summonState = new Summon(this);
    }
    private void OnEnable()
    {
        InitializeStats(); // Fuerza sus 15 de vida al aparecer

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }

        if (TryGetComponent<Collider>(out var mainCollider))
        {
            mainCollider.enabled = true;
        }

        if (weaponCollider != null) weaponCollider.enabled = false;

        // Iniciamos en su propio estado de Idle de Boss
        if (bossIdleState != null)
        {
            ChangeState(bossIdleState);
        }
    }
    protected override void GetInitialState(out TemplateStateMachine _stateMachine)
    {
        _stateMachine = idleState;
    }

    protected override void InitializeStats()
    {
        maxHealth = 15f;       // VIDA DEL BOSS en float
        currentHealth = maxHealth;
        isBoss = true;
    }

    public void RegisterMinion(EnemyFSMManager e)
    {
        aliveMinions++;
        System.Action deathHandler = null;
        deathHandler = () =>
        {
            aliveMinions--;
            e.OnDeath -= deathHandler; // Se desuscribe pa que el pool no acumule basura
        };

        e.OnDeath += deathHandler;
    }

    public void EvaluateWaves()
    {
        if (enemy == null)
            enemy = GetComponent<EnemyFSMManager>();

       
        if (currentWave == 0 && enemy.CurrentHealth <= 10f)
        {
            Debug.Log("[BossManager] EvaluateWaves -> trigger wave 1");
            currentWave = 1;
            ChangeState(summonState);

            return;
        }

       
        if (currentWave == 1 && enemy.CurrentHealth <= 5f)
        {
            Debug.Log("[BossManager] EvaluateWaves -> trigger wave 2");
            currentWave = 2;
            ChangeState(summonState);

            return;
        }
    }

    public override void SystemTakeDamage(float amount)
    {
        if (currentHealth <= 0) return; // Si ya cayó, ignoramos más daño

        // Si el boss tiene el escudo activo, bloquea el golpe por completo
        if (isShielded)
        {
            Debug.Log($"tiene escudo flipao");
            return;
        }

        // Restamos la vida usando la variable heredada
        currentHealth -= amount;
       

        
        if (SoundController.Instance != null)
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.xDamage);
        }

        // 4. Evaluamos inmediatamente las oleadas tras recibir el golpe
        EvaluateWaves();

        // 5. Si la vida llega a 0, ejecutamos su muerte controlada de Boss
        if (currentHealth <= 0)
        {
            BossDie();
        }
    }

    private void BossDie()
    {
        Debug.Log($"El Boss  ha sido derrotado.");

        // Cambiamos inmediatamente al estado Death base para congelar su IA
        ChangeState(deathState);

        if (animatorBoss != null)
        {
            animatorBoss.SetTrigger("IsDead");
        }

        // Frenamos físicas por completo por si acaso
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (TryGetComponent<Collider>(out var mainCollider))
        {
            mainCollider.enabled = false;
        }

       
        // Lo destruimos tras 2 segundos para dar tiempo a ver la animación caer
        Destroy(gameObject, 2f);
    }
}