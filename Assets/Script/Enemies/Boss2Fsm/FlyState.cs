using UnityEngine;

public class FlyState : TemplateStateMachine
{
    PhoenixFSM phoenix;

    public FlyState(string name, PhoenixFSM _stateMachineFlow) : base(name, _stateMachineFlow)
    {
        phoenix = _stateMachineFlow;
    }

    public override void Enter()
    {
        Debug.Log("ENTER Fly");
        phoenix.ResetAire();
    }

    public override void UpdateLogic()
    {
        if (phoenix.jugador != null)
            phoenix.PlayerDistance = Vector3.Distance(phoenix.transform.position, phoenix.jugador.position);

        phoenix.AirTime += Time.deltaTime;

        phoenix.stamina -= Time.deltaTime * 0.05f;
        if (phoenix.stamina < 0f)
            phoenix.stamina = 0f;

        if (phoenix.CurrentPhase == 1 && phoenix.Health <= 0f)
        {
            phoenix.ChangeState(phoenix.phaseTransitionState);
            return;
        }

        if (phoenix.PlayerDistance < 8f)
        {
            phoenix.ChangeState(phoenix.shootState);
            return;
        }

        if (phoenix.stamina < 0.2f)
        {
            phoenix.transitionAirState.goingUp = false;
            phoenix.ChangeState(phoenix.transitionAirState);
            return;
        }

        OrbitalMovement();
        LookAtPlayer();

    }

    void OrbitalMovement()
    {
        if (phoenix.jugador == null) return;

        Vector3 center = phoenix.jugador.position;
        Vector3 offset = phoenix.transform.position - center;

        if (offset.magnitude < 0.1f)
            offset = new Vector3(1f, 0f, 0f);

        Vector3 orbital = Vector3.Cross(offset, Vector3.up).normalized;

        phoenix.transform.position += orbital * 4f * Time.deltaTime;
    }
    void LookAtPlayer()
    {
        if (phoenix.jugador == null) return;

        Vector3 dir = phoenix.jugador.position - phoenix.transform.position;
        dir.y = 0f; // Mantener rotación horizontal

        if (dir.sqrMagnitude > 0.01f)
            phoenix.transform.rotation = Quaternion.LookRotation(dir);
    }

}
