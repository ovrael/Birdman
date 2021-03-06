using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	#region Public Variables
	[SerializeField] bool drawGizmos = true;

	public float attackDistance; //Minimum distance for attack
	public float moveSpeed;
	public float timer; //Timer for cooldown between attacks
	[SerializeField] float leftLimitDistance = 10f;
	[SerializeField] float rightLimitDistance = 10f;
	[HideInInspector] public Transform target;
	[HideInInspector] public bool inRange; //Check if  is in range
	[HideInInspector] public CapsuleCollider2D targetCollider;
	public BoxCollider2D hitBox;
	public GameObject hotZone;
	public GameObject triggerArea;
	#endregion

	#region Private Variables
	Transform leftLimit;
	Transform rightLimit;
	private Animator anim;
	private float distance; //Store the distance b/w enemy and 
	private bool attackMode;
	private bool cooling; //Check if Enemy is cooling after attack
	private float intTimer;
	float damage;
	#endregion

	void Awake()
	{
		//handling right and left limit
		GameObject leftGO = new GameObject();
		GameObject rightGO = new GameObject();
		leftGO.transform.position = new Vector3(gameObject.transform.position.x - leftLimitDistance, gameObject.transform.position.y, gameObject.transform.position.z);
		rightGO.transform.position = new Vector3(gameObject.transform.position.x + rightLimitDistance, gameObject.transform.position.y, gameObject.transform.position.z);

		leftLimit = leftGO.transform;
		rightLimit = rightGO.transform;

		//setting target
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		targetCollider = target.GetComponentInChildren<CapsuleCollider2D>();
		SelectTarget();
		intTimer = timer; //Store the inital value of timer
		anim = GetComponent<Animator>();

		damage = GetComponentInChildren<EnemyStats>().Damage;
	}

	void Update()
	{
		if (!attackMode)
		{
			Move();
		}

		if (!InsideOfLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
		{
			SelectTarget();
		}

		if (inRange)
		{
			EnemyLogic();
		}
	}

	void EnemyLogic()
	{
		distance = Vector2.Distance(transform.position, target.position);

		if (distance > attackDistance)
		{
			StopAttack();
		}
		else if (attackDistance >= distance && cooling == false)
		{
			Attack();
		}

		if (cooling)
		{
			Cooldown();
			anim.SetBool("Attack", false);
		}
	}

	void Move()
	{
		anim.SetBool("canWalk", true);

		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
		{
			Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);

			transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
		}
	}

	void Attack()
	{
		timer = intTimer; //Reset Timer when  enter Attack Range
		attackMode = true; //To check if Enemy can still attack or not

		anim.SetBool("canWalk", false);
		anim.SetBool("Attack", true);

		if (hitBox.IsTouching(targetCollider))
		{
			target.GetComponentInChildren<PlayerStats>().TakeDamage(damage);
			cooling = true;
			Debug.LogWarning("Attack Player for: " + damage);
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
		anim.SetBool("Attack", false);
	}

	public void TriggerCooling()
	{
		cooling = true;
	}

	private bool InsideOfLimits()
	{
		return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
	}

	public void SelectTarget()
	{
		float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
		float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

		if (distanceToLeft > distanceToRight)
		{
			target = leftLimit;
		}
		else
		{
			target = rightLimit;
		}

		//Ternary Operator
		//target = distanceToLeft > distanceToRight ? leftLimit : rightLimit;

		Flip();
	}

	public void Flip()
	{

		if (transform.position.x > target.position.x)
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			transform.localScale = new Vector3(1f, 1f, 1f);
		}

		//Ternary Operator
		//rotation.y = (currentTarget.position.x < transform.position.x) ? rotation.y = 180f : rotation.y = 0f;

	}

	private void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			// Draws a blue line from this transform to the target
			Vector3 bias = new Vector3(0, 1, 0);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(leftLimit.position - bias, leftLimit.position + bias);
			Gizmos.DrawLine(rightLimit.position - bias, rightLimit.position + bias);
		}
	}

	//private void OnCollisionEnter2D(Collision2D collision)
	//{
	//    if (target.CompareTag("Player"))
	//    {

	//        else Debug.LogWarning("nie dzisiaj");
	//    }
	//}
}

