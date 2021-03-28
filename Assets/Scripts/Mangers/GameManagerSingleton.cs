using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSingleton : MonoBehaviour
{
	public static GameManagerSingleton Instance { get; private set; }
	[SerializeField] PlayerStats playerStats;
	[SerializeField] SpellSystem spellSystem;
	[SerializeField] float timeBetweenSaves = 30f;
	private float nextSaveTime;

	private void Save()
	{
		SaveSystem.SavePlayer(playerStats, spellSystem);
	}

	private void Load()
	{
		SavePlayerData playerData = SaveSystem.LoadPlayer();

		if (playerData != null)
		{
			playerStats.MaxHP = playerData.maxHP;
			playerStats.CurrentHP = playerStats.MaxHP;
			playerStats.RegenHP = playerData.regenHP;

			playerStats.MaxMP = playerData.maxMP;
			playerStats.CurrentMP = playerStats.MaxMP;
			playerStats.RegenMP = playerData.regenMP;

			playerStats.Level = playerData.level;
			playerStats.SpellPoints = playerData.spellPoints;

			playerStats.PercentageDamageReduction = playerData.percentageDamageReduction;

			spellSystem.SpellsData = LoadSpells(playerData.spellNames);
		}
		else
		{
			Debug.LogError("Player data is null!");
		}
	}

	private SpellData[] LoadSpells(string[] spellNames)
	{
		SpellData[] returnSpells = new SpellData[3];
		SpellData[] loadedSpells = Resources.LoadAll<SpellData>("");

		int spellIndex = 0;

		for (int i = 0; i < loadedSpells.Length; i++)
		{
			for (int j = 0; j < spellNames.Length; j++)
			{
				if (loadedSpells[i].name == spellNames[j])
				{
					returnSpells[spellIndex] = loadedSpells[i];
					spellIndex++;
					break;
				}
			}
		}

		return returnSpells;
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		Load();
		nextSaveTime = Time.time;
	}

	private void Update()
	{
		if (Time.time > nextSaveTime)
		{
			Debug.Log("GAME SAVED");
			Save();
			nextSaveTime += timeBetweenSaves;
		}
	}
}
