using UnityEngine;

public class TransitionAirState : TemplateStateMachine
{
    PhoenixFSM phoenix;
    float timer;
    float duration = 1.2f;

    public bool goingUp;

    float verticalSpeed = 5f;

    public TransitionAirState(string name, PhoenixFSM _stateMachineFlow) : base(name, _stateMachineFlow)
    {
        phoenix = _stateMachineFlow;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("ENTER  TransitionAir (goingUp: " + goingUp + ")");
        timer = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        timer += Time.deltaTime;

        Vector3 pos = phoenix.transform.position;

        if (goingUp)
            pos += Vector3.up * verticalSpeed * Time.deltaTime;
        else
            pos += Vector3.down * verticalSpeed * Time.deltaTime;

        phoenix.transform.position = pos;

        if (timer >= duration)
        {
            if (goingUp)
                phoenix.ChangeState(phoenix.flyState);
            else
                phoenix.ChangeState(phoenix.groundedState);
        }
    }
}
