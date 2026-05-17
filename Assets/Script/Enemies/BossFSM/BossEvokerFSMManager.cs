using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvokerFSMManager : EnemyFSMManager
{
    private EnemyFSMManager enemy;

    //estados
    public BossIdle bossIdleState;
    public Summon summonState;
    public Shield shieldState;

    //variables
    public int currentWave = 0;
    public int aliveMinions = 0;
    public bool isShielded = false;

    public Animator animatorBoss;


    protected override void Awake() // Usamos override para que vaya fino
    {
        base.Awake();

        // Volvemos a forzar sus stats de Boss para que no herede los de un enemigo comun
        InitializeStats();

        animator = animatorBoss;
        enemy = GetComponent<EnemyFSMManager>();

        // Creamos la instancia específica del boss y la asignamos también al campo base `idleState`
        bossIdleState = new BossIdle(this);
        idleState = bossIdleState;

        shieldState = new Shield(this);
        summonState = new Summon(this);
    }

    protected override void GetInitialState(out TemplateStateMachine _stateMachine)
    {
        _stateMachine = idleState;
    }

    protected override void InitializeStats()
    {
        maxHealth = 10f;       // VIDA DEL BOSS en float
        currentHealth = maxHealth;
        isBoss = true;
    }

    public void RegisterMinion(EnemyFSMManager e)
    {
        aliveMinions++;
        e.OnDeath += () => aliveMinions--;
    }

    public void EvaluateWaves()
    {
        if (enemy == null)
            enemy = GetComponent<EnemyFSMManager>();

        // Primera transición: de wave 0 a 1 (evaluando con floats)
        if (currentWave == 0 && enemy.CurrentHealth <= 4f)
        {
            Debug.Log("[BossManager] EvaluateWaves -> trigger wave 1");
            currentWave = 1;
            ChangeState(summonState);
            return;
        }

        // Segunda transición: de wave 1 a 2 (evaluando con floats)
        if (currentWave == 1 && enemy.CurrentHealth <= 2f)
        {
            Debug.Log("[BossManager] EvaluateWaves -> trigger wave 2");
            currentWave = 2;
            ChangeState(summonState);
            return;
        }
    }
}