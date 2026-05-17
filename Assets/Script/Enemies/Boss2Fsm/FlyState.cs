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
        // medimos la distancia directa contra el player
        if (phoenix.jugador != null)
            phoenix.PlayerDistance = Vector3.Distance(phoenix.transform.position, phoenix.jugador.position);

        phoenix.AirTime += Time.deltaTime;

        // aqui baja la estamina con el multiplicador q tenias puesto
        phoenix.stamina -= Time.deltaTime * 0.05f;
        if (phoenix.stamina < 0f)
            phoenix.stamina = 0f;

        // si se queda sin vida en fase uno pos va a cambiar de fase
        if (phoenix.CurrentPhase == 1 && phoenix.Health <= 0f)
        {
            phoenix.ChangeState(phoenix.phaseTransitionState);
            return;
        }

        // si el player se acerca mucho le mete un tiro
        if (phoenix.PlayerDistance < 8f)
        {
            phoenix.ChangeState(phoenix.shootState);
            return;
        }

        // si se cansa ponemos el goingup en false pa q baje
        if (phoenix.stamina < 0.2f)
        {
            phoenix.transitionAirState.goingUp = false;
            phoenix.ChangeState(phoenix.transitionAirState);
            return;
        }

        OrbitalMovement();
        LookAtPlayer();
    }

    // tu metodo original pa dar vueltas al rededor usando vectores tangenciales
    void OrbitalMovement()
    {
        if (phoenix.jugador == null) return;

        Vector3 center = phoenix.jugador.position;
        Vector3 offset = phoenix.transform.position - center;

        if (offset.magnitude < 0.1f)
            offset = new Vector3(1f, 0f, 0f);

        // el producto cruzado pa sacar la direccion del giro de lao
        Vector3 orbital = Vector3.Cross(offset, Vector3.up).normalized;

        phoenix.transform.position += orbital * 4f * Time.deltaTime;
    }

    // rota el bicho en horizontal acia el player
    void LookAtPlayer()
    {
        if (phoenix.jugador == null) return;

        Vector3 dir = phoenix.jugador.position - phoenix.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
            phoenix.transform.rotation = Quaternion.LookRotation(dir);
    }
}