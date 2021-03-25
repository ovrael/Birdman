using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
	PlayerStats player;

	#region Health
	[Header("Health")]
	[SerializeField] float maxHP = 500;
	[SerializeField] float currentHP = 500;

	// Properties
	public float MaxHP
	{
		get => maxHP;
		set
		{
			if (value > 0)
			{
				maxHP = value;
			}
		}
	}

	public float CurrentHP
	{
		get => currentHP;
		set
		{
			if (value <= maxHP)
			{
				currentHP = value;
			}
			else
			{
				currentHP = maxHP;
			}
		}
	}
	#endregion

	#region Damage
	[Header("Damage")]
	[SerializeField] float damage = 100;

	public float Damage
	{
		get => damage;
		set
		{
			if (value >= 0)
				damage = value;
		}
	}
	#endregion

	#region Experience
	[Header("Experience")]
	[SerializeField] float experienceAfterDeath = 200;

	#endregion

	#region Methods
	public void TakeDamage(float damage)
	{
		currentHP -= damage;
	}
	#endregion

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStats>();
	}

	private void Update()
	{
		if (currentHP <= 0)
		{
			Destroy(gameObject);
			player.CurrentExp += experienceAfterDeath;
		}
	}
}
