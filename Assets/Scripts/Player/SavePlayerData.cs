using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavePlayerData
{
	public float baseHP;
	public float baseRegenHP;
	public float baseMP;
	public float baseRegenMP;
	public float baseArmor;

	public int level;
	public int spellPoints;
	public int passivePoints;

	public string[] spellNames;
	public int[] pickedNodesIds;

	public SavePlayerData(PlayerStats playerStats, SpellSystem spellSystem)
	{
		baseHP = playerStats.Health.BaseValue;
		baseRegenHP = playerStats.RegenHP.BaseValue;
		baseMP = playerStats.Mana.BaseValue;
		baseRegenMP = playerStats.RegenMP.BaseValue;
		baseArmor = playerStats.Armor.BaseValue;

		level = playerStats.Level;
		spellPoints = playerStats.SpellPoints;
		passivePoints = playerStats.PassivePoints;

		spellNames = new string[3];
		for (int i = 0; i < spellSystem.SpellsData.Length; i++)
		{
			if (spellSystem.SpellsData[i] != null)
			{
				spellNames[i] = spellSystem.SpellsData[i].name;
			}
		}

		pickedNodesIds = new int[playerStats.PassiveIds.Count];
		for (int i = 0; i < pickedNodesIds.Length; i++)
		{
			pickedNodesIds[i] = playerStats.PassiveIds[i];
		}
	}
}
