using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static void SavePlayer(PlayerStats playerStats, SpellSystem spellSystem)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/PlayerData.info";
		FileStream fileStream = new FileStream(path, FileMode.Create);

		SavePlayerData playerData = new SavePlayerData(playerStats, spellSystem);

		formatter.Serialize(fileStream, playerData);
		fileStream.Close();
	}

	public static SavePlayerData LoadPlayer()
	{
		string path = Application.persistentDataPath + "/PlayerData.info";
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
}
