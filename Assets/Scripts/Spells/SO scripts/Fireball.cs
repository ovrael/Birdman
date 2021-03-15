using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Fireball")]
public class Fireball : SpellData
{
	[Header("Own stats")]
	public float projectileSpeed = 160f;
}
