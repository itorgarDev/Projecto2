using UnityEngine;

public class Attack : TemplateStateMachine
{
    private EnemyFSMManager _fsm;

    public Attack(EnemyFSMManager stateMachineFlow) : base("Attacking", stateMachineFlow)
    {
        _fsm = stateMachineFlow;
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        float dist = _fsm.DistanceToPlayer();

        // vuelve a idle si canseeplayer es false
        if (!_fsm.CanSeePlayer())
        {
            stateMachineFlow.ChangeState(_fsm.idleState);
            return;
        }

        // vuelve a chase si la distancia no es suficiente
        if (dist > _fsm.attackDistance)
        {
            stateMachineFlow.ChangeState(_fsm.chaseState);
            return;
        }
    }

    public override void Updatephysics()
    {
        base.Updatephysics();

        // Mirar al jugador
        Vector3 dir = (_fsm.player.position - _fsm.transform.position).normalized;
        Vector3 flat = new Vector3(dir.x, 0, dir.z);

        if (flat.sqrMagnitude > 0.001f)
            _fsm.transform.rotation = Quaternion.LookRotation(flat);

        // Ejecutar ataque
        _fsm.enemyAttack.TryAttack();
    }
}
