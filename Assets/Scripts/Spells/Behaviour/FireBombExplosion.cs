using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBombExplosion : MonoBehaviour
{
	[SerializeField] FireBomb spellStats;

	[Range(0.01f, 0.5f)]
	[SerializeField] float speed = 0.1f;
	float startRadius;
	float endRadius;

	void Start()
	{
		startRadius = spellStats.startExplosionRadius;
		endRadius = spellStats.endExplosionRadius.CalculatedValue;

		transform.localScale = new Vector3(startRadius, startRadius, 1);
	}

	void FixedUpdate()
	{
		if (startRadius < endRadius)
		{
			float scale = startRadius;

			transform.localScale = new Vector3(scale, scale, 1);
			startRadius += speed;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			EnemyStats enemy = collision.transform.parent.GetComponentInChildren<EnemyStats>();
			enemy.TakeDamage(spellStats.CalculateDamagePerInstance());
		}
	}
}
