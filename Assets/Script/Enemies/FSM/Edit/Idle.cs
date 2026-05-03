using UnityEngine;

public class Idle : TemplateStateMachine
{
    private EnemyFSMManager _fsm;
   

    public Idle(EnemyFSMManager stateMachineFlow) : base("Idle", stateMachineFlow)
    {
        _fsm = stateMachineFlow;
    }

    public override void Enter()
    {
        base.Enter();
        _fsm.animator.SetBool("isIdle", true);
        _fsm.animator.SetBool("isChasing", false);

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (_fsm.CanSeePlayer())
        {
            stateMachineFlow.ChangeState(_fsm.chaseState);
        }
       

    }


}
