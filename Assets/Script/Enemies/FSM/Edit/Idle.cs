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
