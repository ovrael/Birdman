using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemyBehaviour : MonoBehaviour
{
	#region Public Variables
	public float attackDistance; //Minimum distance for attack
	public float timer; //Timer for cooldown between attacks
	public float timeBetweenAttacks = 2f; //Timer for cooldown between attacks
	public Transform target;
	[HideInInspector] public bool inRange; //Check if  is in range
	public CapsuleCollider2D targetCollider;
	public BoxCollider2D hitBox;
	public AIPath aiPath;
	#endregion

	#region Private Variables
	private float distance; //Store the distance b/w enemy and 
	private bool attackMode;
	private bool cooling; //Check if Enemy is cooling after attack
	private float intTimer;
	PlayerStats playerStats;

	float damage;

	float nextAttackTime;

	#endregion

	void Awake()
	{
		nextAttackTime = Time.time;
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		AIDestinationSetter AIDestinationSetter = GetComponent<AIDestinationSetter>();
		AIDestinationSetter.target = target;
		targetCollider = target.GetComponentInChildren<CapsuleCollider2D>();
		intTimer = timer; //Store the inital value of timer
		damage = GetComponentInChildren<EnemyStats>().Damage;
		playerStats = target.GetComponentInChildren<PlayerStats>();
	}

	void Update()
	{

		if (aiPath.desiredVelocity.x >= 0.01f)
		{
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (aiPath.desiredVelocity.x <= -0.01f)
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		EnemyLogic();
	}

	void EnemyLogic()
	{
		distance = Vector2.Distance(transform.position, target.position);

		if (Time.time > nextAttackTime && distance < attackDistance)
		{
			playerStats.TakeDamage(damage);
			cooling = true;
			nextAttackTime = Time.time + timeBetweenAttacks;
			Debug.LogWarning("Attacked player for: " + damage);
		}
	}


	void Cooldown()
	{
		timer -= Time.deltaTime;

		if (timer <= 0 && cooling && attackMode)
		{
			cooling = false;
			timer = intTimer;
		}
	}

	void StopAttack()
	{
		cooling = false;
		attackMode = false;
	}

	public void TriggerCooling()
	{
		cooling = true;
	}

}
