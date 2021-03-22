using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Arc")]

public class Arc : SpellData
{
	[Header("Own stats")]
	public int jumpsBetweenEnemies;
	public float projectileSpeed;
	public float findEnemyRadius;
	public float attackEnemyRadius;
}
