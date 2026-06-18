using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BossIdle : Idle
{
    private BossEvokerFSMManager bossFsm;
    private EnemyFSMManager enemy;

    public BossIdle(BossEvokerFSMManager fsm) : base(fsm)
    {
        bossFsm = fsm;
        enemy = fsm.GetComponent<EnemyFSMManager>();

    }

    public override void Enter()
    {
        base.Enter();
        bossFsm.animatorBoss.SetBool("isIdle", true);
        bossFsm.animatorBoss.SetBool("isChasing", false);
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

       

        
    }
}
