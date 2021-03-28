using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavePlayerData
{
	public float maxHP;
	public float regenHP;

	public float maxMP;
	public float regenMP;

	public int level;
	public int spellPoints;

	public float percentageDamageReduction;

	public string[] spellNames;

	public SavePlayerData(PlayerStats playerStats, SpellSystem spellSystem)
	{
		maxHP = playerStats.MaxHP;
		regenHP = playerStats.RegenHP;

		maxMP = playerStats.MaxMP;
		regenMP = playerStats.RegenMP;

		level = playerStats.Level;
		spellPoints = playerStats.SpellPoints;

		percentageDamageReduction = playerStats.PercentageDamageReduction;

		spellNames = new string[3];
		for (int i = 0; i < spellSystem.SpellsData.Length; i++)
		{
			if (spellSystem.SpellsData[i] != null)
			{
				spellNames[i] = spellSystem.SpellsData[i].name;
			}
		}
	}
}
