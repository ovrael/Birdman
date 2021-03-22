using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellDamageType
{
	Lightning = 0,
	Fire = 1,
	Water = 2,
	Ice = 3,
	Heal = 11
}

public enum Target
{
	Player = 0,
	NoTarget = 1,
	Enemy = 2
}

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Clear Spell")]
public class SpellData : ScriptableObject
{
	[Header("Description")]
	public new string name;
	public string description;
	public Sprite icon;

	public GameObject prefab;

	[SerializeField] int level = 0;
	[SerializeField] int maxLevel = 20;

	public int Level
	{
		get => level;
		set
		{
			if (value >= 0 && value <= maxLevel)
			{
				level = value;
			}
		}
	}

	public int MaxLevel
	{
		get => maxLevel;
	}

	public float manaCost;

	[Header("Time")]
	public float cooldown;
	public bool isOnCooldown;
	public float duration = 0f;

	[Header("Damage")]
	public Target target;
	public SpellDamageType damageType;

	public float minDamagePerInstance; // If spell does damage overtime it will do x times that damage
	public float maxDamagePerInstance;

	[Header("Damage over time")]
	public bool isDamageOverTime = false;
	public float timeBetweenDamageInstances = 0f;
	public float damageOverTimeDuration = 0f;

	public float CalculateDamagePerInstance()
	{
		return Random.Range(minDamagePerInstance, maxDamagePerInstance);
	}

	public void LevelUp()
	{
		Level++;
	}
}
