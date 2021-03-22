using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
	[SerializeField] Fireball spellStats;

	private Rigidbody2D rb;

	// Start is called before the first frame update
	void Awake()
	{
		transform.Rotate(0, 0, 90);
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		rb.velocity = transform.up * spellStats.projectileSpeed;
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if (collider.gameObject.CompareTag("Enemy"))
		{
			collider.gameObject.GetComponent<EnemyStats>().TakeDamage(spellStats.CalculateDamagePerInstance());
			Destroy(gameObject);
		}

		if (collider.gameObject.CompareTag("Terrain"))
		{
			Destroy(gameObject);
		}
	}
}
