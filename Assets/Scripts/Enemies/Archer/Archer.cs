using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Entity
{
	public Archer_MoveState moveState { get; private set; }
	public Archer_IdleState idleState { get; private set; }
	public Archer_PlayerDetectedState playerDetectedState { get; private set; }
	public Archer_MeleeAttackState meleeAttackState { get; private set; }

	[SerializeField]
	private D_MoveState moveStateData;

	[SerializeField]
	private D_IdleState idleStateData;

	[SerializeField]
	private D_PlayerDetected playerDetectedStateData;

	[SerializeField]
	private D_MeleeAttack meleeAttackStateData;

	[SerializeField]
	private Transform meleeAttackPosition;

	public override void Start()
	{
		base.Start();
		moveState = new Archer_MoveState(this, stateMachine, "move", moveStateData, this);
		idleState = new Archer_IdleState(this, stateMachine, "idle", idleStateData, this);
		playerDetectedState = new Archer_PlayerDetectedState(this, stateMachine, "idle", playerDetectedStateData, this);
		meleeAttackState = new Archer_MeleeAttackState(this, stateMachine, "meleeAttack",meleeAttackPosition, meleeAttackStateData, this);
		stateMachine.Initialize(moveState);
	}

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
