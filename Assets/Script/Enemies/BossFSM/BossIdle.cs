using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BossIdle hereda de Idle y añade la comprobación de vida para las oleadas del boss.
// Mantén los comentarios y los Debug.Log para facilitar la depuración.
public class BossIdle : Idle
{
    private BossEvokerFSMManager bossFsm;
    private Enemy enemy;

    public BossIdle(BossEvokerFSMManager fsm) : base(fsm)
    {
        bossFsm = fsm;
        enemy = fsm.GetComponent<Enemy>();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (bossFsm == null || enemy == null)
            return;

        // Primera transición: de wave 0 a 1
        if (bossFsm.currentWave == 0 && enemy.CurrentHealth <= 4)
        {
            Debug.Log("[BossIdle] Vida <= 4: disparando Summon (wave 1)");
            bossFsm.currentWave = 1;
            stateMachineFlow.ChangeState(bossFsm.summonState);
            return;
        }

        // Segunda transición: de wave 1 a 2
        if (bossFsm.currentWave == 1 && enemy.CurrentHealth <= 2)
        {
            Debug.Log("[BossIdle] Vida <= 2: disparando Summon (wave 2)");
            bossFsm.currentWave = 2;
            stateMachineFlow.ChangeState(bossFsm.summonState);
            return;
        }

        // Resto de la lógica Idle (detección, movimiento, etc.) queda intacta
        // Puedes añadir aquí más Debug.Log si necesitas trazar comportamiento.
    }
}
