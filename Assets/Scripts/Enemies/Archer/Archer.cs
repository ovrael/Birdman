using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Entity
{
	public Archer_MoveState moveState { get; private set; }
	public Archer_IdleState idleState { get; private set; }
	public Archer_PlayerDetectedState playerDetectedState { get; private set; }
	public Archer_MeleeAttackState meleeAttackState { get; private set; }
	public Archer_LookForPlayerState lookForPlayerState { get; private set; }
	public Archer_RangedAttackState rangedAttackState { get; private set; }

	[SerializeField]
	private D_MoveState moveStateData;

	[SerializeField]
	private D_IdleState idleStateData;

	[SerializeField]
	private D_PlayerDetected playerDetectedStateData;

	[SerializeField]
	private D_MeleeAttack meleeAttackStateData;

	[SerializeField]
	private D_LookForPlayer lookForPlayerStateData;

	[SerializeField]
	private D_RangedAttackState rangedAttackStateData;

	[SerializeField]
	private Transform meleeAttackPosition;
	[SerializeField]
	private Transform rangedAttackPosition;

	public override void Start()
	{
		base.Start();
		moveState = new Archer_MoveState(this, stateMachine, "move", moveStateData, this);
		idleState = new Archer_IdleState(this, stateMachine, "idle", idleStateData, this);
		playerDetectedState = new Archer_PlayerDetectedState(this, stateMachine, "idle", playerDetectedStateData, this);
		meleeAttackState = new Archer_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
		lookForPlayerState = new Archer_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
		rangedAttackState = new Archer_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);

		stateMachine.Initialize(moveState);
	}

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);
        if (!CheckPlayerInMinAgroRange())
        {
			lookForPlayerState.SetTurnImmediately(true);
			stateMachine.ChangeState(lookForPlayerState);
        }
		else if (CheckPlayerInMinAgroRange())
        {
			stateMachine.ChangeState(rangedAttackState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
		Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
