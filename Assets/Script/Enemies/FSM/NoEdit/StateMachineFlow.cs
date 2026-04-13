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
        // Asignar un valor a la variable de salida
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
            currentState.Updatephysics();
        }
    }

    public void ChangeState(TemplateStateMachine newState)
    {
        currentState.Exit();
        currentState = newState;
        newState.Enter();
    }

    //Messaging:
    public TMP_Text stateName;
    //OnGui
    public void NamePrint(string name)
    {
        if (currentState == null)
            stateName.text = "Warning: no current state";
        else stateName.text = name;
    }



}
