using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_LookForPlayerState : LookForPlayerState
{
    private Archer archer;
    public Archer_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayer stateData, Archer archer) : base(entity, stateMachine, animBoolName, stateData)
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
        }else if (isAllTurnsDone)
        {
            stateMachine.ChangeState(archer.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
