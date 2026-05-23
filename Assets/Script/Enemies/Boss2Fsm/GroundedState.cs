using UnityEngine;

public class GroundedState : TemplateStateMachine
{
    PhoenixFSM phoenix;

    public GroundedState(string name, PhoenixFSM _stateMachineFlow) : base(name, _stateMachineFlow)
    {
        phoenix = _stateMachineFlow;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("ENTER  Grounded");
        phoenix.ResetTierra();
        phoenix.animator.SetBool("IsFlying", false);
        phoenix.animator.SetBool("IsIdle", true);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        phoenix.GroundTime += Time.deltaTime;

        // recarga la estamina poco a poco
        phoenix.stamina += Time.deltaTime * 0.25f;
        if (phoenix.stamina > 1f)
            phoenix.stamina = 1f;

        // si se recupera, vuelve a subir
        if (phoenix.stamina >= 1f)
        {
            phoenix.transitionAirState.goingUp = true;
            phoenix.ChangeState(phoenix.transitionAirState);
        }
    }
}