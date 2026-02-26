using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : TemplateStateMachine
{
    public FSMManager fsm;
    public Idle(FSMManager _stateMMachineFlow) : base ("Idle", (StateMachineFlow) _stateMMachineFlow)
    {

    }
}
