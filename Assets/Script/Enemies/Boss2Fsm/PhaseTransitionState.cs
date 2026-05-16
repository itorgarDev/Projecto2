using UnityEngine;

public class PhaseTransitionState : TemplateStateMachine
{
    PhoenixFSM phoenix;
    float timer;
    float duration = 2f;

    public PhaseTransitionState(string name, PhoenixFSM _stateMachineFlow) : base(name, _stateMachineFlow)
    {
        phoenix = _stateMachineFlow;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("ENTER  PhaseTransition");
        timer = 0f;

        phoenix.CurrentPhase = 2;
        phoenix.stamina = 1f;
        phoenix.ResetAire();
        phoenix.ResetTierra();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        timer += Time.deltaTime;

        if (timer >= duration)
            phoenix.ChangeState(phoenix.flyState);
    }
}
