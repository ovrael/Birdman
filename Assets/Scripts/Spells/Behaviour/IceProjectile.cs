using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
	[SerializeField] IceCrystal spellStats;

	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{
		rb.velocity = -1 * transform.up * spellStats.projectileSpeed;
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if (collider.gameObject.CompareTag("EnemyObject"))
		{
			collider.gameObject.GetComponentInChildren<EnemyStats>().TakeDamage(spellStats.CalculateDamagePerInstance());
			Destroy(gameObject);
		}

		if (collider.gameObject.CompareTag("Terrain"))
		{
			Destroy(gameObject);
		}
	}
}
