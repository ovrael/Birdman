using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcProjectile : MonoBehaviour
{
	[SerializeField] Arc spellStats;
	[SerializeField] bool drawAttackRange;
	[SerializeField] bool drawFindRange;


	private Rigidbody2D rb;
	private Transform currentTarget;
	private Transform nextTarget;
	private Vector2 startVelocity;
	private EnemyStats attackedEnemy;
	private float destroyTime;
	private int leftJumps;

	// Start is called before the first frame update
	void Awake()
	{
		transform.Rotate(0, 0, 90);

		rb = GetComponent<Rigidbody2D>();
		startVelocity = rb.velocity;
		leftJumps = spellStats.jumpsBetweenEnemies;
	}

	private void Start()
	{
		rb.velocity = transform.up * spellStats.projectileSpeed;
		destroyTime = Time.time + spellStats.duration;
	}


	private void FlyToCurrentTarget()
	{
		Vector2 projectilePos = new Vector2(transform.position.x, transform.position.y);
		Vector2 targetPos = new Vector2(currentTarget.position.x, currentTarget.position.y);

		transform.position = Vector2.MoveTowards(projectilePos, targetPos, spellStats.projectileSpeed * Time.deltaTime);
	}

	void Update()
	{
		if (leftJumps > 0)
		{
			if (currentTarget != null)
			{
				if (Vector3.Distance(transform.position, currentTarget.position) > spellStats.attackEnemyRadius)
				{
					FlyToCurrentTarget();
				}
				else
				{
					AttackEnemy();
					FindNextTarget();
				}
			}
			else
			{
				FindNextTarget();
			}

		}
		else
		{
			Destroy(gameObject);
		}

		if (Time.time > destroyTime && currentTarget == null)
		{
			Destroy(gameObject);
		}
	}

	private void AttackEnemy()
	{
		leftJumps--;
		attackedEnemy.TakeDamage(spellStats.CalculateDamagePerInstance());
	}

	private void FindNextTarget()
	{
		System.Random numberGenerator = new System.Random();
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		List<int> enemiesIndexInRange = new List<int>();

		for (int i = 0; i < enemies.Length; i++)
		{
			if (Vector3.Distance(transform.position, enemies[i].transform.position) <= spellStats.findEnemyRadius)
			{
				bool isTerrainBetween = false;

				List<RaycastHit2D> hits = new List<RaycastHit2D>();
				Physics2D.Linecast(transform.position, enemies[i].transform.position, new ContactFilter2D(), hits);

				for (int j = 0; j < hits.Count; j++)
				{
					if (hits[j].collider.CompareTag("Terrain"))
					{
						isTerrainBetween = true;
						break;
					}
				}

				if (!isTerrainBetween)
				{
					enemiesIndexInRange.Add(i);
				}
			}
		}

		int randomEnemyIndex = numberGenerator.Next(enemiesIndexInRange.Count);

		if (currentTarget == null)
		{
			if (enemiesIndexInRange.Count > 0)
			{
				nextTarget = enemies[enemiesIndexInRange[randomEnemyIndex]].transform;
				currentTarget = nextTarget;

				attackedEnemy = currentTarget.GetComponent<EnemyStats>();

				rb.velocity = startVelocity;

			}
		}
		else
		{
			if (enemiesIndexInRange.Count == 1)
			{
				Destroy(gameObject);
			}
			else
			{
				do
				{
					randomEnemyIndex = numberGenerator.Next(enemiesIndexInRange.Count);
					nextTarget = enemies[enemiesIndexInRange[randomEnemyIndex]].transform;

					if (nextTarget != currentTarget)
					{
						currentTarget = nextTarget;
						attackedEnemy = currentTarget.GetComponent<EnemyStats>();
						break;
					}

				} while (true);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (drawAttackRange)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, spellStats.attackEnemyRadius);
		}

		if (drawFindRange)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, spellStats.findEnemyRadius);
		}
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if (collider.gameObject.CompareTag("Terrain"))
		{
			Destroy(gameObject);
		}
	}

	//private void OnTriggerEnter2D(Collider2D what)
	//{

	//}

	//void OnCollisionEnter2D(Collision2D collider)
	//{
	//	if (collider.gameObject.CompareTag("Enemy"))
	//	{
	//		collider.gameObject.GetComponent<EnemyStats>().TakeDamage(spellStats.CalculateDamagePerInstance());
	//		Destroy(gameObject);
	//	}

	//	if (collider.gameObject.CompareTag("Wall"))
	//	{
	//		Destroy(gameObject);
	//	}
	//}
}
