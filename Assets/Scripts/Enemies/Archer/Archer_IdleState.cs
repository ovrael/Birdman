using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_IdleState : IdleState
{
    private Archer archer;
    public Archer_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Archer archer): base(entity, stateMachine, animBoolName, stateData)
    {
        this.archer = archer;
    }
}
