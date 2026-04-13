using UnityEngine;

public class Chase : TemplateStateMachine
{
    private EnemyFSMManager _fsm;

    public Chase(EnemyFSMManager stateMachineFlow) : base("Chasing", stateMachineFlow)
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

        // pasa a ataque si esta en la distancia adecuada
        if (dist <= _fsm.attackDistance)
        {
            stateMachineFlow.ChangeState(_fsm.attackState);
            return;
        }
    }

    public override void Updatephysics()
    {
        base.Updatephysics();

        float dist = _fsm.DistanceToPlayer();
        if (dist <= _fsm.stopDistance)
            return;

        // Dirección hacia el jugador
        Vector3 dir = (_fsm.player.position - _fsm.transform.position).normalized;
        Vector3 flat = new Vector3(dir.x, 0, dir.z);

        // Movimiento
        _fsm.rb.MovePosition(_fsm.transform.position + flat * _fsm.chaseSpeed * Time.deltaTime);

        // Rotación hacia el jugador
        if (flat.sqrMagnitude > 0.001f)
            _fsm.transform.rotation = Quaternion.LookRotation(flat);
    }
}
