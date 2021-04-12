using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSpellsData
{
	public string[] names;
	public string[] descriptions; // DELETE TEST
	public string[] createdDescriptions;

	public int[] levels;

	public float[] baseManaCosts;
	public float[] baseCooldowns;
	public float[] baseDurations;

	public float[] baseMinDamagePerInstance;
	public float[] baseMaxDamagePerInstance;

	public float[][] customData;

	public SaveSpellsData(SpellData[] spells)
	{
		int spellsCount = spells.Length;

		names = new string[spellsCount];
		descriptions = new string[spellsCount];
		createdDescriptions = new string[spellsCount];

		levels = new int[spellsCount];

		baseManaCosts = new float[spellsCount];
		baseCooldowns = new float[spellsCount];
		baseDurations = new float[spellsCount];

		baseMinDamagePerInstance = new float[spellsCount];
		baseMaxDamagePerInstance = new float[spellsCount];

		customData = new float[spellsCount][];


		for (int i = 0; i < spellsCount; i++)
		{
			names[i] = spells[i].name;
			descriptions[i] = spells[i].description;
			createdDescriptions[i] = spells[i].createdDescription;

			levels[i] = spells[i].Level;

			baseManaCosts[i] = spells[i].manaCost.BaseValue;
			baseCooldowns[i] = spells[i].cooldown.BaseValue;
			baseDurations[i] = spells[i].duration.BaseValue;

			baseMinDamagePerInstance[i] = spells[i].minDamagePerInstance.BaseValue;
			baseMaxDamagePerInstance[i] = spells[i].maxDamagePerInstance.BaseValue;

			float[] data = spells[i].GetCustomData();
			customData[i] = new float[data.Length];
			customData[i] = data;

		}
	}
}
