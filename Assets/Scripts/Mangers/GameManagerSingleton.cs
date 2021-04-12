using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSingleton : MonoBehaviour
{
	public static GameManagerSingleton Instance { get; private set; }

	[Header("Player Data")]
	[SerializeField] PlayerStats playerStats;
	[SerializeField] SpellSystem spellSystem;
	[SerializeField] PassivesManager passiveManager;

	[Header("Save system")]
	[SerializeField] GameObject saveCanvas;
	[SerializeField] float timeBetweenSaves = 30f;
	[SerializeField] bool saveGameNow = false;

	private float nextSaveTime;

	public void Save()
	{
		SaveSystem.SavePlayer(playerStats, spellSystem);

		if (passiveManager == null)
		{
			passiveManager = FindObjectOfType<PassivesManager>();
		}

		if (passiveManager != null)
			SaveSystem.SaveSpells(passiveManager.allSpells);

		StartCoroutine(ShowSaveText());
	}

	IEnumerator ShowSaveText()
	{
		saveCanvas.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		saveCanvas.SetActive(false);
	}

	private void Load()
	{
		SavePlayerData playerData = SaveSystem.LoadPlayer();

		if (playerData != null)
		{
			playerStats.Health = new Stat(playerData.baseHP);
			playerStats.CurrentHP = playerStats.Health.CalculatedValue;

			playerStats.RegenHP = new Stat(playerData.baseRegenHP);

			if (playerData.baseMP == 0)
				playerData.baseMP = 350;

			playerStats.Mana = new Stat(playerData.baseMP);
			playerStats.CurrentMP = playerStats.Mana.CalculatedValue;

			playerStats.RegenMP = new Stat(playerData.baseRegenMP);

			playerStats.Level = playerData.level;
			playerStats.SpellPoints = playerData.spellPoints;
			playerStats.PassivePoints = playerData.passivePoints;

			playerStats.Armor = new Stat(playerData.baseArmor);

			spellSystem.SpellsData = LoadSpells(playerData.spellNames);

			if (playerData.pickedNodesIds != null)
			{
				LoadPassiveNodes(playerData.pickedNodesIds);
			}
			else
				playerStats.PassiveIds = new List<int>();
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

		SaveSpellsData spellsData = SaveSystem.LoadSpells();

		List<string> spellNamesList = new List<string>(spellsData.names);

		if (spellsData == null)
		{
			spellsData = new SaveSpellsData(new SpellData[0]);
			Debug.LogError("Spells data is null, created clear spells data object");
		}

		for (int i = 0; i < loadedSpells.Length; i++)
		{
			for (int j = 0; j < spellNamesList.Count; j++)
			{
				if (loadedSpells[i].name == spellNamesList[j])
				{
					SpellData loadedSpell = loadedSpells[i];

					loadedSpell.Level = 0;
					loadedSpell.description = spellsData.descriptions[j];
					loadedSpell.createdDescription = spellsData.createdDescriptions[j];

					loadedSpell.manaCost = new Stat(spellsData.baseManaCosts[j]);
					loadedSpell.cooldown = new Stat(spellsData.baseCooldowns[j]);
					loadedSpell.duration = new Stat(spellsData.baseDurations[j]);

					loadedSpell.minDamagePerInstance = new Stat(spellsData.baseMinDamagePerInstance[j]);
					loadedSpell.maxDamagePerInstance = new Stat(spellsData.baseMaxDamagePerInstance[j]);

					loadedSpell.SetCustomData(spellsData.customData[j]);

					for (int k = 0; k < spellsData.levels[j]; k++)
					{
						loadedSpell.LevelUp();
					}
				}
			}

			for (int j = 0; j < spellNames.Length; j++)
			{
				if (loadedSpells[i].name == spellNames[j])
				{
					if (loadedSpells[i].Level > 0)
						returnSpells[j] = loadedSpells[i];

					break;
				}
			}
		}



		return returnSpells;
	}

	private bool ContainsPickedPassive(int[] pickedPassives, int passiveID)
	{
		bool contains = false;

		foreach (var picked in pickedPassives)
		{
			if (picked == passiveID)
			{
				contains = true;
				break;
			}
		}

		return contains;
	}

	private void LoadPassiveNodes(int[] pickedPassives)
	{
		PassiveNodeScript[] loadedPassives = Resources.LoadAll<PassiveNodeScript>("");

		foreach (PassiveNodeScript passive in loadedPassives)
		{
			if (ContainsPickedPassive(pickedPassives, passive.id))
			{
				passiveManager.ApplyPassiveNode(passive);
			}
		}
	}

	private void FindReferences()
	{
		if (playerStats == null)
		{
			playerStats = FindObjectOfType<PlayerStats>();
		}

		if (spellSystem == null)
		{
			spellSystem = FindObjectOfType<SpellSystem>();
		}

		if (passiveManager == null && SceneChanger.InMenu)
		{
			var objects = Resources.FindObjectsOfTypeAll<PassivesManager>();

			if (objects.Length > 0)
				passiveManager = objects[0];
		}
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

		saveCanvas.SetActive(false);
	}

	void Start()
	{
		Load();
		nextSaveTime = Time.time + timeBetweenSaves;
	}

	private void Update()
	{
		FindReferences();

		if (SceneChanger.InMenu)
		{
			if (Time.time > nextSaveTime)
			{
				Debug.Log("GAME SAVED");
				Save();
				nextSaveTime += timeBetweenSaves;
			}
		}

		if (saveGameNow)
		{
			Debug.Log("GAME SAVED");
			Save();
			saveGameNow = false;
		}
	}
}
