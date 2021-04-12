using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/IceCrystal")]
public class IceCrystal : SpellData
{
	[Header("Own stats")]
	public Stat projectileSpawnTime;
	public float projectileSpeed;

	public float setUpFlySpeed;
	public float setUpFlyTime;

	[Header("Own level up income")]
	public float lessProjectileSpawnTime;

	private readonly int customDataCount = 1;
	public override float[] GetCustomData()
	{
		float[] customData = new float[customDataCount];
		customData[0] = projectileSpawnTime.BaseValue;

		return customData;
	}

	public override void SetCustomData(float[] customData)
	{
		if (customData.Length == customDataCount)
		{
			projectileSpawnTime = new Stat(customData[0], 0, 0);
		}
	}

	public override void LevelUp()
	{
		if (Level > 0)
			if (projectileSpawnTime.CalculatedValue - lessProjectileSpawnTime > 0.2f)
				projectileSpawnTime -= new Stat(0, lessProjectileSpawnTime, 0);

		base.LevelUp();
	}
	public override void CreateDescription()
	{
		StringBuilder info = new StringBuilder();
		info.Append("Ice projectile deals ");
		info.Append(minDamagePerInstance.CalculatedValue.ToString("0.00"));
		info.Append(" to ");
		info.Append(maxDamagePerInstance.CalculatedValue.ToString("0.00"));
		info.AppendLine(" damage.");
		info.Append("Ice Crystal creates one projectile per ");
		info.Append(projectileSpawnTime.CalculatedValue.ToString("0.00"));
		info.Append(" seconds. ");

		createdDescription = info.ToString();
	}
}
