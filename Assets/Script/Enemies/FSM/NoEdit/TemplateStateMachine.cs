using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateStateMachine
{
    public string name;
    protected StateMachineFlow stateMachineFlow;
    public TemplateStateMachine(string name, StateMachineFlow _stateMachineFlow)
    {
        this.name = name;
        this.stateMachineFlow = _stateMachineFlow;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void Updatephysics() { }
    public virtual void Exit() { }
}

