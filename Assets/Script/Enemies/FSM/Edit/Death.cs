using UnityEngine;

public class Death : TemplateStateMachine
{
    private EnemyFSMManager _fsm;

    public Death(EnemyFSMManager stateMachineFlow) : base("Death", stateMachineFlow)
    {
        _fsm = stateMachineFlow;
    }

    public override void Enter()
    {
        base.Enter();

        // Forzamos a que los animadores de movimiento se apaguen
        _fsm.animator.SetBool("isIdle", false);
        _fsm.animator.SetBool("isChasing", false);

        // Desactivamos el collider de ataque inmediatamente al entrar en muerte
        if (_fsm.weaponCollider != null) _fsm.weaponCollider.enabled = false;
    }

    public override void UpdateLogic()
    {
        // No hacemos  nada  El bicho esta muerto.
    }

    public override void Updatephysics()
    {
        // igual que arriba
    }
}