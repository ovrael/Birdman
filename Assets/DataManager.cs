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
		oldPlayerStats.MaxHP = playerStats.MaxHP;
		oldPlayerStats.CurrentHP = oldPlayerStats.MaxHP;
		oldPlayerStats.RegenHP = playerStats.RegenHP;

		oldPlayerStats.MaxMP = playerStats.MaxMP;
		oldPlayerStats.CurrentMP = oldPlayerStats.MaxMP;
		oldPlayerStats.RegenMP = playerStats.RegenMP;

		oldPlayerStats.ExpNeededToLevelUp = playerStats.ExpNeededToLevelUp;
		oldPlayerStats.CurrentExp = 0;
		oldPlayerStats.Level = playerStats.Level;
		oldPlayerStats.SpellPoints = playerStats.SpellPoints;

		oldPlayerStats.SetUp();
	}
}
