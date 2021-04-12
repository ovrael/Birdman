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
	bool tookDamage = false;

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
	[SerializeField] int experienceAfterDeath = 200;

	#endregion

	#region Material and colors
	[Header("Material and colors")]
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Material spriteMaterial;
	[SerializeField] Color takeDamageTint;
	Color currentTint;
	[SerializeField] float tintFadeSpeed;
	#endregion

	#region Methods
	public void TakeDamage(float damage)
	{
		currentHP -= damage;
		tookDamage = true;
	}

	#endregion

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStats>();
		spriteRenderer.material = new Material(spriteMaterial);
		currentTint.a = 0;
		spriteMaterial.SetColor("_Tint", currentTint);
	}

	private void Update()
	{
		if (currentHP <= 0)
		{
			Destroy(transform.parent.gameObject);
			player.CurrentExp += experienceAfterDeath;
		}

		if (tookDamage)
		{
			currentTint = takeDamageTint;
			tookDamage = false;
		}

		if (currentTint.a > 0)
		{
			currentTint.a = Mathf.Clamp01(currentTint.a - tintFadeSpeed * Time.deltaTime);
			spriteRenderer.material.SetColor("_Tint", currentTint);
		}
	}
}
