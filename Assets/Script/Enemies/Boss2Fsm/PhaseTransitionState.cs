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
        SoundController.Instance.PlaySFX(SoundController.Instance.zExplosion);

        if (phoenix.phaseTransitionParticles != null)
        {
            phoenix.phaseTransitionParticles.gameObject.SetActive(true);
            phoenix.phaseTransitionParticles.Play();
        }

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
           
            phoenix.transitionAirState.goingUp = true;

           
            phoenix.ChangeState(phoenix.transitionAirState);
        }
    }

    public override void Exit()
    {
        base.Exit();

       
        if (phoenix.phaseTransitionParticles != null)
        {
            phoenix.phaseTransitionParticles.Stop();
            phoenix.phaseTransitionParticles.gameObject.SetActive(false);
        }
    }
}