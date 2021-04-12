using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLightningProjectile : MonoBehaviour
{
	[SerializeField] StraightLightning spellStats;

	private Rigidbody2D rb;

	// Start is called before the first frame update
	void Awake()
	{

		transform.Rotate(0, 0, 90);

		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		Destroy(gameObject, spellStats.duration.CalculatedValue);
		rb.velocity = transform.up * spellStats.projectileSpeed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Terrain"))
		{
			Destroy(gameObject);
		}

		if (collision.gameObject.CompareTag("Enemy"))
		{
			EnemyStats enemy = collision.transform.parent.GetComponentInChildren<EnemyStats>();
			enemy.TakeDamage(spellStats.CalculateDamagePerInstance());
		}
	}
}
