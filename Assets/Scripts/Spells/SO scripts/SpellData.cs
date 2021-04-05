using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum SpellDamageType
{
	Lightning = 0,
	Fire = 1,
	Water = 2,
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
	[TextArea] public string description;
	public string createdDescription;
	public Sprite icon;

	public GameObject prefab;

	[SerializeField] int level;
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

	public Stat manaCost;

	[Header("Time")]
	public Stat cooldown;
	public bool isOnCooldown;
	public Stat duration;

	[Header("Damage")]
	public Target target;
	public SpellDamageType damageType;
	public Stat minDamagePerInstance;
	public Stat maxDamagePerInstance;

	[Header("Level up income")]
	public float moreManaCost;
	public float moreDuration;
	public float moreMinDamage;
	public float moreMaxDamage;


	public float CalculateDamagePerInstance()
	{
		return Random.Range(minDamagePerInstance.CalculatedValue, maxDamagePerInstance.CalculatedValue);
	}

	public virtual float[] GetCustomData()
	{
		return new float[0];
	}

	public virtual void SetCustomData(float[] customData)
	{
	}

	public virtual void LevelUp()
	{
		if (Level > 0)
		{
			manaCost += new Stat(0, moreManaCost, 0);
			duration += new Stat(0, moreDuration, 0);
			minDamagePerInstance += new Stat(0, moreMinDamage, 0);
			maxDamagePerInstance += new Stat(0, moreMaxDamage, 0);
		}

		Level++;

		CreateDescription();
	}

	public virtual void CreateDescription()
	{
		StringBuilder info = new StringBuilder();
		info.Append("Deals ");
		info.Append(minDamagePerInstance.CalculatedValue.ToString("0.00"));
		info.Append(" to ");
		info.Append(maxDamagePerInstance.CalculatedValue.ToString("0.00"));
		info.Append(" damage.");

		createdDescription = info.ToString();
	}

	public void ResetSpellPassives()
	{
		cooldown = new Stat(cooldown.BaseValue);
		duration = new Stat(duration.BaseValue);
		minDamagePerInstance = new Stat(minDamagePerInstance.BaseValue);
		maxDamagePerInstance = new Stat(maxDamagePerInstance.BaseValue);
	}

	public void ResetSpellLevel()
	{
		level = 0;
	}
}
