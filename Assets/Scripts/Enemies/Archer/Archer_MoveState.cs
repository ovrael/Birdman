using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_MoveState : MoveState
{
    private Archer archer;

    public Archer_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Archer archer) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.archer = archer;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(archer.playerDetectedState);
        }
        else if(isDetectingWall || !isDetectingLedge)
        {
            archer.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(archer.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
