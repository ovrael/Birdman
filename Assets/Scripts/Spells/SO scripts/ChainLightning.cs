using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/ChainLightning")]

public class ChainLightning : SpellData
{
	[Header("Own stats")]
	public Stat jumpsBetweenEnemies;
	public float projectileSpeed;
	public float findEnemyRadius;
	public float attackEnemyRadius;

	[Header("Own level up income")]
	public int moreJumps;

	private readonly int customDataCount = 1;

	public override float[] GetCustomData()
	{
		float[] customData = new float[customDataCount];
		customData[0] = jumpsBetweenEnemies.BaseValue;

		return customData;
	}

	public override void SetCustomData(float[] customData)
	{
		if (customData.Length == customDataCount)
		{
			jumpsBetweenEnemies = new Stat(customData[0], 0, 0);
		}
	}


	public override void LevelUp()
	{
		if (Level > 0)
			jumpsBetweenEnemies += new Stat(0, moreJumps, 0);

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
		info.Append(jumpsBetweenEnemies.CalculatedValue.ToString("0"));
		info.Append(" enemies. ");

		createdDescription = info.ToString();
	}
}
