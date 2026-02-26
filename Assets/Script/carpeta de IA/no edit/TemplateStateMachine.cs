using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateStateMachine : MonoBehaviour
{
    public string name;
    protected StateMachineFlow stateMachineFlow;

    public TemplateStateMachine(string name, StateMachineFlow stateMachineFlow)
    {
        this.name = name;
        this.stateMachineFlow = stateMachineFlow;
    }
    public virtual void Enter() { }
    public virtual void UpdateLogic(){}
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}
