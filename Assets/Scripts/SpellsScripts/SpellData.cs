using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellDamageType
{
	Lightning = 0,
	Fire = 1,
	Cold = 2
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
	[Header("Shared stats")]
	public string spellName;
	public string description;

	public Sprite icon;

	public float manaCost;

	[Header("Time")]
	public float cooldown;
	public bool isOnCooldown;
	public float spellDuration = 0f;

	[Header("Damage")]
	public Target target;
	public SpellDamageType damageType;

	public float minDamagePerInstance; // If spell does damage overtime it will do x times that damage
	public float maxDamagePerInstance;

	[Header("Damage over time")]
	public bool isDamageOverTime = false;
	public float timeBetweenDamageInstances = 0f;
	public float damageOverTimeDuration = 0f;


	private readonly System.Random numberGenerator = new System.Random();

	public float CalculateDamagePerInstance()
	{
		return (float)numberGenerator.NextDouble() * (maxDamagePerInstance - minDamagePerInstance) + minDamagePerInstance;
	}
}
