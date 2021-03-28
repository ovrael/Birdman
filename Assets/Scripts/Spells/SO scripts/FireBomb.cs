using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/FireBomb")]
public class FireBomb : SpellData
{
	[Header("Own stats")]
	[SerializeField] float startExplosionRadius;
	[SerializeField] float endExplosionRadius;
	[SerializeField] float explosionDuration;

	public float StartExplosionRadius { get => startExplosionRadius; }
	public float ExplosionDuration { get => ExplosionDuration; }
	public float EndExplosionRadius { get => endExplosionRadius; }
}
