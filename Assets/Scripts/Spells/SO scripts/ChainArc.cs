using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/ChainArc")]

public class ChainArc : SpellData
{
	[Header("Own stats")]
	public int jumpsBetweenEnemies;
	public float projectileSpeed;
	public float findEnemyRadius;
	public float attackEnemyRadius;

	[Header("Own level up income")]
	public int moreJumps;

	public override void LevelUp()
	{
		jumpsBetweenEnemies += moreJumps;
		base.LevelUp();
	}

	public override void CreateDescription()
	{
		StringBuilder info = new StringBuilder();
		info.Append("Arc lightning deals ");
		info.Append(minDamagePerInstance.CalculatedValue.ToString("0.00"));
		info.Append(" to ");
		info.Append(maxDamagePerInstance.CalculatedValue.ToString("0.00"));
		info.Append(" damage per hit. ");
		info.Append("Strokes  ");
		info.Append(jumpsBetweenEnemies);
		info.Append(" enemies. ");

		createdDescription = info.ToString();
	}
}
