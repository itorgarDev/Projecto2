using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMManager : StateMachineFlow
{
   public Idle idleState;

    private void Awake()
    {
        idleState = new Idle(this); 
    }
}
