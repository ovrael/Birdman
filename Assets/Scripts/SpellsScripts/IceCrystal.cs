using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/IceCrystal")]
public class IceCrystal : SpellData
{
	[Header("Own stats")]
	public float projectileSpawnTime;
	public float projectileSpeed;

	public float setUpFlySpeed;
	public float setUpFlyTime;
}
