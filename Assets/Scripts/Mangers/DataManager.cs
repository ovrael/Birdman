using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	private static PlayerStats playerStats;
	private static SpellSystem spellSystem;

	public static void GetPlayerData(PlayerStats playerStats, SpellSystem spellSystem)
	{
		DataManager.playerStats = playerStats;
		DataManager.spellSystem = spellSystem;
	}

	public static void AssignPlayerSpellSystem(GameObject player)
	{
		SpellSystem newSpellSystem = player.GetComponentInChildren<SpellSystem>();

		newSpellSystem.SpellsData = spellSystem.SpellsData;
	}

	public static void AssignPlayerStats(GameObject player)
	{
		PlayerStats newPlayerStats = player.GetComponentInChildren<PlayerStats>();

		newPlayerStats.Health = playerStats.Health;
		newPlayerStats.CurrentHP = newPlayerStats.Health.CalculatedValue;
		newPlayerStats.RegenHP = playerStats.RegenHP;

		newPlayerStats.Mana = playerStats.Mana;
		newPlayerStats.CurrentMP = newPlayerStats.Mana.CalculatedValue;
		newPlayerStats.RegenMP = playerStats.RegenMP;

		newPlayerStats.Armor = playerStats.Armor;

		newPlayerStats.CurrentExp = 0;
		newPlayerStats.Level = playerStats.Level;
		newPlayerStats.SpellPoints = playerStats.SpellPoints;
		newPlayerStats.PassivePoints = playerStats.PassivePoints;
		newPlayerStats.PassiveIds = new List<int>(playerStats.PassiveIds);

		PassiveNodeScript[] loadedPassives = Resources.LoadAll<PassiveNodeScript>("");

		foreach (var passive in loadedPassives)
		{
			if (newPlayerStats.PassiveIds.Contains(passive.id))
				passive.isPicked = true;
		}

		// newPlayerStats.SetUp();
	}
}
