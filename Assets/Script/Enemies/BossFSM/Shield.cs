using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : TemplateStateMachine
{
    private BossEvokerFSMManager _fsm;
    private Enemy _life;

    private GameObject _shieldSphere;
    public Shield(BossEvokerFSMManager stateMachineFlow) : base("Shield", stateMachineFlow)
    {
        _fsm = stateMachineFlow;
        _life = _fsm.GetComponent<Enemy>();
        _shieldSphere = _fsm.transform.Find("ShieldSphere").gameObject;
    }
    public override void Enter()
    {
        base.Enter();

        // Activar escudo visual
        _shieldSphere.SetActive(true);

        // Hacer al boss inmortal
        _fsm.isShielded = true;

        // Parar movimiento
        _fsm.rb.velocity = Vector3.zero;
        _fsm.rb.angularVelocity = Vector3.zero;
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // Si no hay minions vuelve a Idle
        if (_fsm.aliveMinions == 0)
        {
            stateMachineFlow.ChangeState(_fsm.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Desactivar escudo visual
        _shieldSphere.SetActive(false);

        // Volver a activar vida
        _fsm.isShielded = false;
    }
}
