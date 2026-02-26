using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateMachineFlow : MonoBehaviour
{
    TemplateStateMachine currentState;
    void Start()
    {
        GetInitialState(out currentState);

        if (currentState != null)
        {
            currentState.Enter();
        }
    }
    protected virtual void GetInitialState(out TemplateStateMachine _stateMachine)
    {
        _stateMachine = null;
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateLogic();
        }
    }

    void LateUpdate()
    {
        if (currentState != null)
        {
            currentState.UpdatePhysics();
        }
    }

    public void ChangeState(TemplateStateMachine _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

    public TMP_Text stateName;
}
