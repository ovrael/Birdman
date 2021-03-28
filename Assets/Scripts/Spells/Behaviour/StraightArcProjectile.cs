﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightArcProjectile : MonoBehaviour
{
	[SerializeField] StraightArc spellStats;

	private Rigidbody2D rb;

	// Start is called before the first frame update
	void Awake()
	{

		transform.Rotate(0, 0, 90);

		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		Destroy(gameObject, spellStats.duration);
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
			EnemyStats enemy = collision.gameObject.GetComponent<EnemyStats>();
			enemy.TakeDamage(spellStats.CalculateDamagePerInstance());
		}
	}
}
