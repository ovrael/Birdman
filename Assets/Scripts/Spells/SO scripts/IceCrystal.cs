using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/IceCrystal")]
public class IceCrystal : SpellData
{
	[Header("Own stats")]
	public float projectileSpawnTime;
	public float projectileSpeed;

	public float setUpFlySpeed;
	public float setUpFlyTime;

	[Header("Own level up income")]
	public float lessProjectileSpawnTime;

	public override void LevelUp()
	{
		if (projectileSpawnTime - lessProjectileSpawnTime > 0.1f)
			projectileSpawnTime -= lessProjectileSpawnTime;

		base.LevelUp();
	}
	public override void CreateDescription()
	{
		StringBuilder info = new StringBuilder();
		info.Append("Ice projectile deals ");
		info.Append(minDamagePerInstance.CalculatedValue.ToString("0.00"));
		info.Append(" to ");
		info.Append(maxDamagePerInstance.CalculatedValue.ToString("0.00"));
		info.Append(" damage. ");
		info.Append("Ice Crystal creates one projectile per ");
		info.Append(projectileSpawnTime.ToString("0.00"));
		info.Append(" seconds. ");

		createdDescription = info.ToString();
	}
}
