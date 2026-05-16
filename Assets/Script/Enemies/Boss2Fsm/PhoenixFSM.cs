using UnityEngine;

public class PhoenixFSM : StateMachineFlow
{
    public FlyState flyState;
    public ShootState shootState;
    public TransitionAirState transitionAirState;
    public GroundedState groundedState;
    public PhaseTransitionState phaseTransitionState;

    [Header("Variables internas del Fénix")]
    [Range(0f, 1f)]
    public float stamina = 1f;
    public float AirTime = 0f;
    public float GroundTime = 0f;
    public float PlayerDistance = 0f;
    public float GroundDamageRecieve = 0f;
    public int CurrentPhase = 1;

    [Header("Referencias")]
    public Transform jugador;

    public float Health = 15f;

    protected override void GetInitialState(out TemplateStateMachine _stateMachine)
    {
        flyState = new FlyState("Fly", this);
        shootState = new ShootState("Shoot", this);
        transitionAirState = new TransitionAirState("TransitionAir", this);
        groundedState = new GroundedState("Grounded", this);
        phaseTransitionState = new PhaseTransitionState("PhaseTransition", this);

        _stateMachine = flyState;
    }

    public void ResetAire()
    {
        AirTime = 0f;
    }

    public void ResetTierra()
    {
        GroundTime = 0f;
        GroundDamageRecieve = 0f;
    }
}
