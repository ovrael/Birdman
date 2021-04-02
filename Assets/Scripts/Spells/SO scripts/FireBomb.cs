using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/FireBomb")]
public class FireBomb : SpellData
{
	[Header("Own stats")]
	public float startExplosionRadius;
	public float endExplosionRadius;
	public float explosionDuration;

	[Header("Own level up income")]
	public float moreEndExplosionRadius;

	public override void LevelUp()
	{
		endExplosionRadius += moreEndExplosionRadius;
		base.LevelUp();
	}

	public override void CreateDescription()
	{
		base.CreateDescription();
		StringBuilder info = new StringBuilder(createdDescription);

		info.Append("Explosion radius:  ");
		info.Append(endExplosionRadius);
		info.Append(".");

		createdDescription = info.ToString();
	}
}
