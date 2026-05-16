using UnityEngine;

public class ShootState : TemplateStateMachine
{
    PhoenixFSM phoenix;
    float shootTimer;
    float shootDuration = 1f;

    public ShootState(string name, PhoenixFSM _stateMachineFlow) : base(name, _stateMachineFlow)
    {
        phoenix = _stateMachineFlow;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("ENTER Shoot");
        shootTimer = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootDuration)
            phoenix.ChangeState(phoenix.flyState);
    }
}
