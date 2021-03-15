using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/ForwardProjectile")]
public class ForwardProjectile : SpellData
{
	[Header("Own stats")]
	public float projectileSpeed = 160f;
}
