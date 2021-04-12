using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/FireBomb")]
public class FireBomb : SpellData
{
	[Header("Own stats")]
	public float startExplosionRadius;
	public Stat endExplosionRadius;
	public float explosionDuration;

	[Header("Own level up income")]
	public float moreEndExplosionRadius;

	private readonly int customDataCount = 1;
	public override float[] GetCustomData()
	{
		float[] customData = new float[customDataCount];
		customData[0] = endExplosionRadius.BaseValue;

		return customData;
	}

	public override void SetCustomData(float[] customData)
	{
		if (customData.Length == customDataCount)
		{
			endExplosionRadius = new Stat(customData[0], 0, 0);
		}
	}

	public override void LevelUp()
	{
		if (Level > 0)
			endExplosionRadius += new Stat(0, moreEndExplosionRadius, 0);

		base.LevelUp();
	}

	public override void CreateDescription()
	{
		base.CreateDescription();
		StringBuilder info = new StringBuilder(createdDescription);

		info.Append("Explosion radius:  ");
		info.Append(endExplosionRadius.CalculatedValue.ToString("0.00"));
		info.Append(".");

		createdDescription = info.ToString();
	}
}
