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
		Destroy(gameObject, spellStats.duration.CalculatedValue);
		rb.velocity = transform.up * spellStats.projectileSpeed;
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if (collider.gameObject.CompareTag("EnemyObject"))
		{
			collider.transform.GetComponentInChildren<EnemyStats>().TakeDamage(spellStats.CalculateDamagePerInstance());
			Destroy(gameObject);
		}

		if (collider.gameObject.CompareTag("Terrain"))
		{
			Destroy(gameObject);
		}
	}
}
