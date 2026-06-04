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
        phoenix.animator.SetTrigger("Rest");
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

        // si va para arriba, si que usamos el tiempo de subida normal
        if (goingUp)
        {
            if (timer >= duration)
            {
                phoenix.ChangeState(phoenix.flyState);
            }
        }
        // si va para abajo, usamos el raycast para tocar el suelo real
        else
        {
            RaycastHit hit;
            // tiramos el rayo hacia abajo un par de metros
            if (Physics.Raycast(phoenix.transform.position, Vector3.down, out hit, 20f))
            {
                Debug.DrawRay(phoenix.transform.position, Vector3.down * 20, Color.red);
                // si la distancia al suelo real es muy xikitita, es q ya a aterrizado
                if (hit.distance <= 0.3f)
                {
                    phoenix.ChangeState(phoenix.groundedState);
                }
            }
            // por si acaso el mapa se rompe, dejamos el temporizador de emergencia
            else if (timer >= duration * 2f)
            {
                phoenix.ChangeState(phoenix.groundedState);
            }
        }
    }
}