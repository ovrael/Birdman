using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
	#region Health
	[Header("Health")]

	// Available in Unity
	[SerializeField] float maxHP = 500;
	[SerializeField] float currentHP = 500;

	// Properties
	public float MaxHp
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
		}
	}


	#endregion

	public void TakeDamage(float damage)
	{
		currentHP -= damage;
	}

	private void Update()
	{
		if (currentHP <= 0)
		{
			Destroy(gameObject);
		}
	}
}
