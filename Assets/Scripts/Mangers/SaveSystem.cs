using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	private static string playerSaveFile = @"/PlayerData.info";
	private static string spellsSaveFile = @"/SpellsData.info";

	public static void SavePlayer(PlayerStats playerStats, SpellSystem spellSystem)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + playerSaveFile;
		FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

		SavePlayerData playerData = new SavePlayerData(playerStats, spellSystem);

		formatter.Serialize(fileStream, playerData);
		fileStream.Close();
	}

	public static SavePlayerData LoadPlayer()
	{
		string path = Application.persistentDataPath + playerSaveFile;
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream fileStream = new FileStream(path, FileMode.Open);

			SavePlayerData playerData = (SavePlayerData)formatter.Deserialize(fileStream);
			fileStream.Close();

			return playerData;
		}
		else
		{
			Debug.LogError("File not found in " + path);
			return null;
		}
	}

	public static void SaveSpells(SpellData[] spells)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + spellsSaveFile;
		FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

		SaveSpellsData spellsData = new SaveSpellsData(spells);

		formatter.Serialize(fileStream, spellsData);
		fileStream.Close();
	}
	public static SaveSpellsData LoadSpells()
	{
		string path = Application.persistentDataPath + spellsSaveFile;
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream fileStream = new FileStream(path, FileMode.Open);

			SaveSpellsData spellsData = (SaveSpellsData)formatter.Deserialize(fileStream);
			fileStream.Close();

			return spellsData;
		}
		else
		{
			Debug.LogError("File not found in " + path);
			return null;
		}
	}

	public static void DeleteData()
	{
		string playerPath = Application.persistentDataPath + playerSaveFile;
		string spellsPath = Application.persistentDataPath + spellsSaveFile;

		File.Delete(playerPath);
		File.Delete(spellsPath);
	}
}
