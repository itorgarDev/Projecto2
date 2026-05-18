using UnityEngine;

public class FlyState : TemplateStateMachine
{
    PhoenixFSM phoenix;

    // variables pal tiempo de recarga del tiro
    float shootCooldownTimer = 0f;
    float timeBetweenAttacks = 3f; // se tiene k esperar 3 segundos entre atakes

    public FlyState(string name, PhoenixFSM _stateMachineFlow) : base(name, _stateMachineFlow)
    {
        phoenix = _stateMachineFlow; //
    }

    public override void Enter()
    {
        Debug.Log("ENTER Fly"); //
        phoenix.ResetAire(); //

        // cada vez k vuelve a volar, reiniciamos el reloj de recarga pa k no tire instantaneo
        shootCooldownTimer = 0f; //
    }

    public override void UpdateLogic()
    {
        // medimos la distancia directa contra el player
        if (phoenix.target != null) //
            phoenix.PlayerDistance = Vector3.Distance(phoenix.transform.position, phoenix.target.position); //

        phoenix.AirTime += Time.deltaTime; //

        // aqui baja la estamina con el multiplicador q tenias puesto
        phoenix.stamina -= Time.deltaTime * 0.05f; //
        if (phoenix.stamina < 0f) //
            phoenix.stamina = 0f; //

        // si se queda sin vida en fase uno pos va a cambiar de fase
        if (phoenix.CurrentPhase == 1 && phoenix.Health <= 0f) //
        {
            phoenix.ChangeState(phoenix.phaseTransitionState); //
            return; //
        }

        // --- EL ARREGLO DEL BUCLE ---
        // aumentamos el reloj de la recarga
        shootCooldownTimer += Time.deltaTime; //

        // MODIFICADO: Ahora evalúa usando la variable expuesta del Inspector (maxShootRange) en vez del '8f' fijo
        if (phoenix.PlayerDistance < phoenix.maxShootRange && shootCooldownTimer >= timeBetweenAttacks) //
        {
            phoenix.ChangeState(phoenix.shootState); //
            return; //
        }

        // si se cansa ponemos el goingup en false pa q baje
        if (phoenix.stamina < 0.2f) //
        {
            phoenix.transitionAirState.goingUp = false; //
            phoenix.ChangeState(phoenix.transitionAirState); //
            return; //
        }

        OrbitalMovement(); //
        LookAtPlayer(); //
    }

    void OrbitalMovement() //
    {
        if (phoenix.target == null) return; //

        Vector3 center = phoenix.target.position; //
        Vector3 offset = phoenix.transform.position - center; //

        if (offset.magnitude < 0.1f) //
            offset = new Vector3(1f, 0f, 0f); //

        Vector3 orbital = Vector3.Cross(offset, Vector3.up).normalized; //

        phoenix.transform.position += orbital * 4f * Time.deltaTime; //
    }

    void LookAtPlayer() //
    {
        if (phoenix.target == null) return; //

        Vector3 dir = phoenix.target.position - phoenix.transform.position; //
        dir.y = 0f; //

        if (dir.sqrMagnitude > 0.01f) //
            phoenix.transform.rotation = Quaternion.LookRotation(dir); //
    }
}