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
		SpellSystem oldSpellSystem = player.GetComponentInChildren<SpellSystem>();

		oldSpellSystem.SpellsData = spellSystem.SpellsData;
	}

	public static void AssignPlayerStats(GameObject player)
	{
		PlayerStats oldPlayerStats = player.GetComponentInChildren<PlayerStats>();

		oldPlayerStats.Health = playerStats.Health;
		oldPlayerStats.CurrentHP = oldPlayerStats.Health.CalculatedValue;
		oldPlayerStats.RegenHP = playerStats.RegenHP;

		oldPlayerStats.Mana = playerStats.Mana;
		oldPlayerStats.CurrentMP = oldPlayerStats.Mana.CalculatedValue;
		oldPlayerStats.RegenMP = playerStats.RegenMP;

		oldPlayerStats.Armor = playerStats.Armor;

		oldPlayerStats.CurrentExp = 0;
		oldPlayerStats.Level = playerStats.Level;
		oldPlayerStats.SpellPoints = playerStats.SpellPoints;
		oldPlayerStats.PassivePoints = playerStats.PassivePoints;
		oldPlayerStats.PassiveIds = new List<int>(playerStats.PassiveIds);

		PassiveNodeScript[] loadedPassives = Resources.LoadAll<PassiveNodeScript>("");

		foreach (var passive in loadedPassives)
		{
			if (oldPlayerStats.PassiveIds.Contains(passive.id))
				passive.isPicked = true;
		}

		oldPlayerStats.SetUp();
	}
}
