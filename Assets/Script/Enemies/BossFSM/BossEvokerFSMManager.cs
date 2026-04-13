using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvokerFSMManager : EnemyFSMManager
{
    private Enemy enemy;

    //estados
    public BossIdle bossIdleState;
    public Summon summonState;
    public Shield shieldState;

    //variables
    public int currentWave = 0;
    public int aliveMinions = 0;
    public bool isShielded = false;


    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();

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

    public void RegisterMinion(Enemy e)
    {
        aliveMinions++;
        e.OnDeath += () => aliveMinions--;
    }
    public void EvaluateWaves()
    {
        if (enemy == null)
            enemy = GetComponent<Enemy>();

        // Primera transición: de wave 0 a 1
        if (currentWave == 0 && enemy.CurrentHealth <= 4)
        {
            Debug.Log("[BossManager] EvaluateWaves -> trigger wave 1");
            currentWave = 1;
            ChangeState(summonState);
            return;
        }

        // Segunda transición: de wave 1 a 2
        if (currentWave == 1 && enemy.CurrentHealth <= 2)
        {
            Debug.Log("[BossManager] EvaluateWaves -> trigger wave 2");
            currentWave = 2;
            ChangeState(summonState);
            return;
        }
    }

}
