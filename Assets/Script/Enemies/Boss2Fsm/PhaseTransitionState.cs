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

        Debug.Log("[Fenix] ENTER Fase2");
        timer = 0f;

        phoenix.CurrentPhase = 2;

        // Dejamos la estamina a 1f para que cuando llegue al cielo este a tope,
        // pero reseteamos tierra porque el bicho revive en el suelo.
        phoenix.stamina = 1f;

        // esto cura al fenix a tope pa la fase dos asi no se rompe la fsm
        phoenix.Health = phoenix.maxHealth;

        // Como esta en el suelo durante la animacion de resurgir, usamos ResetTierra
        phoenix.ResetTierra();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        timer += Time.deltaTime;

        if (timer >= duration)
        {
            // ¡REPARADO! En vez de mandarlo a volar directo en el suelo,
            // le decimos al estado de transicion que tiene que SUBIR.
            phoenix.transitionAirState.goingUp = true;

            // Lo mandamos al estado que se encarga de moverlo hacia el cielo fisicamente
            phoenix.ChangeState(phoenix.transitionAirState);
        }
    }
}